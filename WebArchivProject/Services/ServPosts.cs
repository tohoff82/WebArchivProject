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
    class ServPosts : IServPosts
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IRepoPosts _repoPosts;
        private readonly IRepoAuthors _repoAuthors;
        private readonly IServUserSession _userSession;
        private readonly PagerSettings _pagerSettings;

        /// <summary>
        /// Ключ получения кеша Статей
        /// </summary>
        private string KeyId => string
            .Format("Posts_{0}", _userSession.User.Id);

        /// <summary>
        /// Ключ получения кеша для комбо фильтров статей
        /// </summary>
        private string FilterId => string
            .Format("Posts_Filter_{0}", _userSession.User.Id);

        /// <summary>
        /// Ключ получения кеша для результата поиска по фильтру статей
        /// </summary>
        private string SearchId => string
            .Format("Posts_Search_{0}", _userSession.User.Id);

        private SessionUser User
            => _userSession.User;

        public ServPosts(
            IMapper mapper,
            IMemoryCache cache,
            IRepoPosts repoPosts,
            IRepoAuthors repoAuthors,
            IServUserSession userSession,
            IOptions<MySettings> options)
        {
            _mapper = mapper;
            _cache = cache;
            _repoPosts = repoPosts;
            _repoAuthors = repoAuthors;
            _userSession = userSession;
            _pagerSettings = options.Value.PagerSettings;
        }

        /// <summary>
        /// Добавление статьи в БД
        /// </summary>
        /// <param name="dtoPost">ДТО объекта статьи</param>
        public async Task AddToDbAsync(DtoPost dtoPost)
        {
            string guid = Guid.NewGuid().ToString();

            var post = _mapper.Map<Post>(dtoPost);
            post.AuthorExternalId = guid;
            post.OwnerId = User.Id;
            await _repoPosts.AddPostAsync(post);

            var authors = _mapper.Map<List<Author>>(dtoPost.Authors);
            await _repoAuthors.AddAuthorsRangeAsync(authors.With(guid));

            await UpdatePostsCashAsync();
            await UpdatePostsFiltersCashAsync();
        }

        /// <summary>
        /// Удаление статьи из БД
        /// </summary>
        /// <param name="postId">идентификатор</param>
        public async Task DeleteFromDbAsync(int postId)
        {
            var post = await _repoPosts.GetPostByIdAsync(postId);
            var authors = await _repoAuthors.GetAuthorsByExtIdAsync(post.AuthorExternalId);

            await _repoPosts.DeletePostAsync(post);
            await _repoAuthors.DeleteAuthorsRangeAsync(authors);

            await UpdatePostsCashAsync();
            await UpdatePostsFiltersCashAsync();

            if (GetSearchCash() != null) RemoveFromPostsSearchCash(postId);
        }

        /// <summary>
        /// Результат фильтрации книг
        /// </summary>
        /// <param name="filter">объект параметров фильра</param>
        /// <returns>Объект результатов поиска</returns>
        public Paginator<DtoSearchresultPost> GetPaginatorResultModal(PostsSearchFilter filter)
        {
            UpdatePostsSearchCashAsync(filter).GetAwaiter().GetResult();
            return GetPostsSearchPaginator(1, _pagerSettings.ItemPerPage, filter.Target);
        }

        /// <summary>
        /// Результат отображения всех статей (пагинация)
        /// </summary>
        /// <param name="pageNumber">страница</param>
        /// <param name="pageSize">количество элементов на страницу</param>
        /// <returns>Объект отображаемого результата</returns>
        public Paginator<DtoSearchresultPost> GetPaginationResult(int pageNumber, int pageSize, string target)
        {
            if (GetPostsCash() == null) UpdatePostsCashAsync().GetAwaiter().GetResult();
            var paginationResult = GetPostsPaginator(pageNumber, pageSize);
            paginationResult.ForContainer = target;
            paginationResult.ForTable = POST;
            return paginationResult;
        }

        /// <summary>
        /// Получения данных для фильтра статей
        /// </summary>
        public PostsComboFilters GetPostsComboFilters()
        {
            if (GetFiltersCash() == null) UpdatePostsFiltersCashAsync().GetAwaiter().GetResult();
            return GetFiltersCash();
        }

        /// <summary>
        /// Создания объекта пагинации всех статей
        /// </summary>
        /// <param name="pageNumber">страница</param>
        /// <param name="pageSize">количество элементов на страницу</param>
        private Paginator<DtoSearchresultPost> GetPostsPaginator(int pageNumber, int pageSize)
            => Paginator<DtoSearchresultPost>.ToList(GetPostsCash(), pageNumber, pageSize);

        /// <summary>
        /// Создание объекта пагинации для фильтра статей
        /// </summary>
        public Paginator<DtoSearchresultPost> GetPostsSearchPaginator(int pageNumber, int pageSize, string target)
        {
            var paginationResult = Paginator<DtoSearchresultPost>.ToList(GetSearchCash(), pageNumber, pageSize);
            paginationResult.ForContainer = target;
            paginationResult.ForTable = POST;
            return paginationResult;
        }

        /// <summary>
        /// Обновление кеша всех статей
        /// </summary>
        private async Task UpdatePostsCashAsync()
        {
            var posts = new List<DtoSearchresultPost>();
            foreach (var item in await _repoPosts.ToListAsync())
            {
                var authors = await _repoAuthors
                    .GetAuthorsByExtIdAsync(
                        item.AuthorExternalId);

                var post = _mapper.Map<DtoSearchresultPost>(item);
                post.Authors = authors.Select(a => a.NameUa).ToList();
                posts.Add(post);
            }

            _cache.Remove(KeyId);

            _cache.Set(KeyId, posts, new MemoryCacheEntryOptions
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
        private async Task UpdatePostsFiltersCashAsync()
        {
            var posts = await _repoPosts.ToListAsync();
            var authors = await _repoAuthors.ToListAsync();

            _cache.Remove(FilterId);

            _cache.Set(FilterId,
            new PostsComboFilters
            {
                Years = posts.OrderBy(b => b.Year)
                    .GroupBy(b => b.Year).Select(b
                        => b.Key).ToList(),
                Names = posts.OrderBy(b => b.Name)
                    .Select(b => b.Name).ToList(),
                Magazine = posts.OrderBy(b => b.Magazine)
                    .GroupBy(m => m.Magazine).Select(m
                        => m.Key).ToList(),
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
        private async Task UpdatePostsSearchCashAsync(PostsSearchFilter filter)
        {
            var posts = new List<DtoSearchresultPost>();
            var filteredPosts = await _repoPosts.FilteredPostsToListAsync
            (
                year: filter.PostYear,
                name: filter.PostName,
                magazine: filter.Magazine
            );

            if (filteredPosts != null)
            {
                foreach (var filterPost in filteredPosts)
                {
                    var authors = await _repoAuthors
                    .GetAuthorsByExtIdAsync(
                        filterPost.AuthorExternalId);

                    var post = _mapper.Map<DtoSearchresultPost>(filterPost);
                    post.Authors = authors.Select(l => l.NameUa).ToList();
                    post.Authors.OrderBy(a => a[0]).ToList();
                    posts.Add(post);
                }
            }

            _cache.Remove(SearchId);

            _cache.Set(SearchId, filter.AuthorName == DEFAULT_FILTER ? posts : posts
                    .DtoPostFilterByAuthor(filter.AuthorName.ToNameUa()),
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds
                (
                    value: _userSession.User.Expirate - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                )
            });
        }

        /// <summary>
        /// Удаления объекта статьи из отфильтрованного кеша
        /// </summary>
        /// <param name="postId">идентификатор</param>
        private void RemoveFromPostsSearchCash(int postId)
        {
            var list = GetSearchCash();
            var post = list.FirstOrDefault(p => p.Id == postId);
            if (post != null) list.Remove(post);
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
        /// Получение кеша всех статей
        /// </summary>
        private List<DtoSearchresultPost> GetPostsCash()
        {
            object obj = _cache.Get(KeyId);
            return obj as List<DtoSearchresultPost>;
        }

        /// <summary>
        /// Получение кеша комбо фильтров
        /// </summary>
        private PostsComboFilters GetFiltersCash()
        {
            object obj = _cache.Get(FilterId);
            return obj as PostsComboFilters;
        }

        /// <summary>
        /// Получения кеша отфильтрованых статей
        /// </summary>
        public List<DtoSearchresultPost> GetSearchCash()
        {
            object obj = _cache.Get(SearchId);
            return obj as List<DtoSearchresultPost>;
        }
    }
}
