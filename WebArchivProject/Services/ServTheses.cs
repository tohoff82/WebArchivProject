using AutoMapper;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;
using WebArchivProject.Models;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Models.DTO;
using WebArchivProject.Models.SearchFilters;
using WebArchivProject.Models.VO;

using static WebArchivProject.Helper.StringConstant;

namespace WebArchivProject.Services
{
    class ServTheses : IServTheses
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IRepoTheses _repoTheses;
        private readonly IRepoAuthors _repoAuthors;
        private readonly IServUserSession _userSession;
        private readonly PagerSettings _pagerSettings;

        /// <summary>
        /// Ключ получения кеша тезисов
        /// </summary>
        private string KeyId => string
            .Format("Theses_{0}", _userSession.User.Id);

        /// <summary>
        /// Ключ получения кеша для комбо фильтров тезисов
        /// </summary>
        private string FilterId => string
            .Format("Theses_Filter_{0}", _userSession.User.Id);

        /// <summary>
        /// Ключ получения кеша для результата поиска по фильтру тезисов
        /// </summary>
        private string SearchId => string
            .Format("Theses_Search_{0}", _userSession.User.Id);

        private SessionUser User
            => _userSession.User;

        public ServTheses(
            IMapper mapper,
            IMemoryCache cache,
            IRepoTheses repoTheses,
            IRepoAuthors repoAuthors,
            IServUserSession userSession,
            IOptions<MySettings> options)
        {
            _mapper = mapper;
            _cache = cache;
            _repoTheses = repoTheses;
            _repoAuthors = repoAuthors;
            _userSession = userSession;
            _pagerSettings = options.Value.PagerSettings;
        }

        /// <summary>
        /// Добавление тезиса в БД
        /// </summary>
        /// <param name="dtoThesis">ДТО объекта статьи</param>
        public async Task AddToDbAsync(DtoThesis dtoThesis)
        {
            string guid = Guid.NewGuid().ToString();

            var thesis = _mapper.Map<Thesis>(dtoThesis);
            thesis.AuthorExternalId = guid;
            thesis.OwnerId = User.Id;
            await _repoTheses.AddThesisAsync(thesis);

            var authors = _mapper.Map<List<Author>>(dtoThesis.Authors);
            await _repoAuthors.AddAuthorsRangeAsync(authors.With(guid));

            await UpdateThesesCashAsync();
            await UpdateThesesFiltersCashAsync();
        }

        /// <summary>
        /// Удаление тезиса из БД
        /// </summary>
        /// <param name="postId">идентификатор</param>
        public async Task DeleteFromDbAsync(int thesisId)
        {
            var thesis = await _repoTheses.GetThesisByIdAsync(thesisId);
            var authors = await _repoAuthors.GetAuthorsByExtIdAsync(thesis.AuthorExternalId);

            await _repoTheses.DeleteThesisAsync(thesis);
            await _repoAuthors.DeleteAuthorsRangeAsync(authors);

            await UpdateThesesCashAsync();
            await UpdateThesesFiltersCashAsync();

            if (GetSearchCash() != null) RemoveFromThesesSearchCash(thesisId);
        }

        /// <summary>
        /// Результат фильтрации тезисов
        /// </summary>
        /// <param name="filter">объект параметров фильра</param>
        /// <returns>Объект результатов поиска</returns>
        public Paginator<DtoSearchresultThesis> GetPaginatorResultModal(ThesesSearchFilter filter)
        {
            UpdateThesesSearchCashAsync(filter).GetAwaiter().GetResult();
            return GetThesesSearchPaginator(1, _pagerSettings.ItemPerPage, filter.Target);
        }

        /// <summary>
        /// Результат отображения всех тезисов (пагинация)
        /// </summary>
        /// <param name="pageNumber">страница</param>
        /// <param name="pageSize">количество элементов на страницу</param>
        /// <returns>Объект отображаемого результата</returns>
        public Paginator<DtoSearchresultThesis> GetPaginationResult(int pageNumber, int pageSize, string target)
        {
            if (GetThesesCash() == null) UpdateThesesCashAsync().GetAwaiter().GetResult();
            var paginationResult = GetThesesPaginator(pageNumber, pageSize);
            paginationResult.ForContainer = target;
            paginationResult.ForTable = THESIS;
            paginationResult.Size = "pagination-sm";
            return paginationResult;
        }

        /// <summary>
        /// Получения данных для фильтра тезисов
        /// </summary>
        public ThesesComboFilters GetThesesComboFilters()
        {
            if (GetFiltersCash() == null) UpdateThesesFiltersCashAsync().GetAwaiter().GetResult();
            return GetFiltersCash();
        }

        /// <summary>
        /// Создания объекта пагинации всех тезисов
        /// </summary>
        /// <param name="pageNumber">страница</param>
        /// <param name="pageSize">количество элементов на страницу</param>
        private Paginator<DtoSearchresultThesis> GetThesesPaginator(int pageNumber, int pageSize)
            => Paginator<DtoSearchresultThesis>.ToList(GetThesesCash(), pageNumber, pageSize);

        /// <summary>
        /// Создание объекта пагинации для фильтра тезисов
        /// </summary>
        public Paginator<DtoSearchresultThesis> GetThesesSearchPaginator(int pageNumber, int pageSize, string target)
        {
            var paginationResult = Paginator<DtoSearchresultThesis>.ToList(GetSearchCash(), pageNumber, pageSize);
            paginationResult.ForContainer = target;
            paginationResult.ForTable = THESIS;
            paginationResult.Size = "pagination-sm";
            return paginationResult;
        }

        /// <summary>
        /// Обновление кеша всех тезисов
        /// </summary>
        private async Task UpdateThesesCashAsync()
        {
            var theses = new List<DtoSearchresultThesis>();
            foreach (var item in await _repoTheses.ToListAsync())
            {
                var authors = await _repoAuthors
                    .GetAuthorsByExtIdAsync(
                        item.AuthorExternalId);

                var thesis = _mapper.Map<DtoSearchresultThesis>(item);
                thesis.Authors = authors.Select(a => a.NameUa).ToList();
                theses.Add(thesis);
            }

            _cache.Remove(KeyId);

            _cache.Set(KeyId, theses, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds
                (
                    value: _userSession.User.Expirate - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                )
            });
        }

        /// <summary>
        /// Обновления кеша фильтров
        /// </summary>
        private async Task UpdateThesesFiltersCashAsync()
        {
            var theses = await _repoTheses.ToListAsync();
            var authors = await _repoAuthors.ToListAsync();

            _cache.Remove(FilterId);

            _cache.Set(FilterId,
            new ThesesComboFilters
            {
                Years = theses.OrderBy(b => b.Year)
                    .GroupBy(b => b.Year).Select(b
                        => b.Key).ToList(),
                Names = theses.OrderBy(b => b.Name)
                    .Select(b => b.Name).ToList(),
                Pages = theses.OrderBy(b => b.PagesInterval)
                    .Select(b => b.PagesInterval).ToList(),
                Authors = authors.OrderBy(a => a.NameUa)
                    .GroupBy(a => a.NameUa).Select(n
                        => n.ToFilterName()).ToList()
            },
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds
                (
                    value: _userSession.User.Expirate - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                )
            });
        }

        /// <summary>
        /// Обновление результатов фильтра
        /// </summary>
        /// <param name="filter">параметры фильтра</param>
        private async Task UpdateThesesSearchCashAsync(ThesesSearchFilter filter)
        {
            var theses = new List<DtoSearchresultThesis>();
            var filteredTheses = await _repoTheses.FilteredThesesToListAsync
            (
                year: filter.ThesisYear,
                name: filter.ThesisName,
                pages: filter.Pages
            );

            if (filteredTheses != null)
            {
                foreach (var filterThesis in filteredTheses)
                {
                    var authors = await _repoAuthors
                    .GetAuthorsByExtIdAsync(
                        filterThesis.AuthorExternalId);

                    var thesis = _mapper.Map<DtoSearchresultThesis>(filterThesis);
                    thesis.Authors = authors.Select(l => l.NameUa).ToList();
                    thesis.Authors.OrderBy(a => a[0]).ToList();
                    theses.Add(thesis);
                }
            }

            _cache.Remove(SearchId);

            _cache.Set(SearchId, filter.AuthorName == DEFAULT_FILTER ? theses : theses
                    .DtoThesisFilterByAuthor(filter.AuthorName.ToNameUa()),
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds
                (
                    value: _userSession.User.Expirate - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                )
            });
        }

        /// <summary>
        /// Удаления объекта тезисов из отфильтрованного кеша
        /// </summary>
        /// <param name="thesisId">идентификатор</param>
        private void RemoveFromThesesSearchCash(int thesisId)
        {
            var list = GetSearchCash();
            var thesis = list.FirstOrDefault(p => p.Id == thesisId);
            if (thesis != null) list.Remove(thesis);
            _cache.Remove(SearchId);
            _cache.Set(SearchId, list,
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds
                (
                    value: _userSession.User.Expirate - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                )
            });
        }

        /// <summary>
        /// Получение кеша всех тезисов
        /// </summary>
        private List<DtoSearchresultThesis> GetThesesCash()
        {
            object obj = _cache.Get(KeyId);
            return obj as List<DtoSearchresultThesis>;
        }

        /// <summary>
        /// Получение кеша комбо фильтров
        /// </summary>
        private ThesesComboFilters GetFiltersCash()
        {
            object obj = _cache.Get(FilterId);
            return obj as ThesesComboFilters;
        }

        /// <summary>
        /// Получения кеша отфильтрованых тезисов
        /// </summary>
        public List<DtoSearchresultThesis> GetSearchCash()
        {
            object obj = _cache.Get(SearchId);
            return obj as List<DtoSearchresultThesis>;
        }
    }
}
