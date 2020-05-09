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
    class ServBooks : IServBooks
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IRepoBooks _repoBooks;
        private readonly IRepoAuthors _repoAuthors;
        private readonly IServUserSession _userSession;
        private readonly PagerSettings _pagerSettings;

        /// <summary>
        /// Ключ получения кеша книг
        /// </summary>
        private string KeyId => string
            .Format("Books_{0}", _userSession.User.Id);

        /// <summary>
        /// ключ получения кеша для комбо фильтров
        /// </summary>
        private string FilterId => string
            .Format("Books_Filter_{0}", _userSession.User.Id);

        /// <summary>
        /// ключ получения кеша для результата поиска по фильтру
        /// </summary>
        private string SearchId => string
            .Format("Books_Search_{0}", _userSession.User.Id);

        private SessionUser User
            => _userSession.User;

        public ServBooks(
            IMapper mapper,
            IMemoryCache cache,
            IRepoBooks repoBooks,
            IRepoAuthors repoAuthors,
            IServUserSession userSession,
            IOptions<MySettings> options)
        {
            _mapper = mapper;
            _cache = cache;
            _repoBooks = repoBooks;
            _repoAuthors = repoAuthors;
            _userSession = userSession;
            _pagerSettings = options.Value.PagerSettings;
        }

        /// <summary>
        /// Добавление книги в БД
        /// </summary>
        /// <param name="dtoBook">ДТО объекта книги</param>
        public async Task AddToDbAsync(DtoBook dtoBook)
        {
            string guid = Guid.NewGuid().ToString();

            var book = _mapper.Map<Book>(dtoBook);
            book.AuthorExternalId = guid;
            book.OwnerId = User.Id;
            await _repoBooks.AddBookAsync(book);

            var authors = _mapper.Map<List<Author>>(dtoBook.Authors);
            await _repoAuthors.AddAuthorsRangeAsync(authors.With(guid));

            await UpdateBooksCashAsync();
            await UpdateBooksFiltersCashAsync();
        }

        /// <summary>
        /// Удаление книги из БД
        /// </summary>
        /// <param name="bookId">айдишник книги</param>
        public async Task DeleteFromDbAsync(int bookId)
        {
            var book = await _repoBooks.GetBookByIdAsync(bookId);
            var authors = await _repoAuthors.GetAuthorsByExtIdAsync(book.AuthorExternalId);

            await _repoBooks.DeleteBookAsync(book);
            await _repoAuthors.DeleteAuthorsRangeAsync(authors);

            await UpdateBooksCashAsync();
            await UpdateBooksFiltersCashAsync();
            if (GetSearchCash() != null) RemoveFromBooksSearchCash(bookId);
        }

        /// <summary>
        /// Результат фильтрации книг
        /// </summary>
        /// <param name="filter">объект параметров фильра</param>
        /// <returns>Объект результатов поиска</returns>
        public Paginator<DtoSearchresultBook> GetPaginatorResultModal(BooksSearchFilter filter)
        {
            UpdateBooksSearchCashAsync(filter).GetAwaiter().GetResult();
            return GetBooksSearchPaginator(1, _pagerSettings.ItemPerPage, filter.Target);
        }

        /// <summary>
        /// Результат отображения всех книг (пагинация)
        /// </summary>
        /// <param name="pageNumber">страница</param>
        /// <param name="pageSize">количество элементов на страницу</param>
        /// <returns>Объект отображаемого результата</returns>
        public Paginator<DtoSearchresultBook> GetPaginationResult(int pageNumber, int pageSize, string target)
        {
            if (GetBooksCash() == null) UpdateBooksCashAsync().GetAwaiter().GetResult();
            var paginationResult = GetBooksPaginator(pageNumber, pageSize);
            paginationResult.ForContainer = target;
            paginationResult.ForTable = BOOK;
            return paginationResult;
        }

        /// <summary>
        /// Получения данных для фильтра книг
        /// </summary>
        public BooksComboFilters GetBooksComboFilters()
        {
            if (GetFiltersCash() == null) UpdateBooksFiltersCashAsync().GetAwaiter().GetResult();
            return GetFiltersCash();
        }

        /// <summary>
        /// Создания объекта пагинации всех книг
        /// </summary>
        /// <param name="pageNumber">страница</param>
        /// <param name="pageSize">количество элементов на страницу</param>
        /// <returns></returns>
        private Paginator<DtoSearchresultBook> GetBooksPaginator(int pageNumber, int pageSize)
            => Paginator<DtoSearchresultBook>.ToList(GetBooksCash(), pageNumber, pageSize);

        /// <summary>
        /// Создание объекта пагинации для фильтра книг
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Paginator<DtoSearchresultBook> GetBooksSearchPaginator(int pageNumber, int pageSize, string target)
        {
            var paginationResult =  Paginator<DtoSearchresultBook>.ToList(GetSearchCash(), pageNumber, pageSize);
            paginationResult.ForContainer = target;
            paginationResult.ForTable = BOOK;
            return paginationResult;
        }

        /// <summary>
        /// Обновление кеша всех книг
        /// </summary>
        private async Task UpdateBooksCashAsync()
        {
            var books = new List<DtoSearchresultBook>();
            foreach (var item in await _repoBooks.ToListAsync())
            {
                var authors = await _repoAuthors
                    .GetAuthorsByExtIdAsync(
                        item.AuthorExternalId);

                var book = _mapper.Map<DtoSearchresultBook>(item);
                book.AuthorFirst = authors.First().NameUa;
                book.AuthorsNext = authors.NextAuthors();
                books.Add(book);
            }

            _cache.Remove(KeyId);

            _cache.Set(KeyId, books, new MemoryCacheEntryOptions
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
        private async Task UpdateBooksFiltersCashAsync()
        {
            var books = await _repoBooks.ToListAsync();
            var authors = await _repoAuthors.ToListAsync();

            _cache.Remove(FilterId);

            _cache.Set(FilterId,
            new BooksComboFilters
            {
                Years = books.OrderBy(b => b.Year)
                    .GroupBy(b => b.Year).Select(b
                        => b.Key).ToList(),
                Names = books.OrderBy(b => b.Name)
                    .Select(b => b.Name).ToList(),
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
        private async Task UpdateBooksSearchCashAsync(BooksSearchFilter filter)
        {
            var books = new List<DtoSearchresultBook>();
            var filteredBooks = await _repoBooks.FilteredBooksToListAsync
            (
                year: filter.BookYear,
                name: filter.BookName
            );

            if (filteredBooks != null)
            {
                foreach (var filterBook in filteredBooks)
                {
                    var authors = await _repoAuthors
                    .GetAuthorsByExtIdAsync(
                        filterBook.AuthorExternalId);

                    var book = _mapper.Map<DtoSearchresultBook>(filterBook);
                    book.AuthorFirst = authors.First().NameUa;
                    book.AuthorsNext = authors.NextAuthors();
                    books.Add(book);
                }
            }

            _cache.Remove(SearchId);

            _cache.Set(SearchId, filter.AuthorName == DEFAULT_FILTER ? books : books
                    .FilterByAuthor(filter.AuthorName.ToNameUa()),
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds
                (
                    value: _userSession.User.Expirate - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                )
            });
        }

        private void RemoveFromBooksSearchCash(int bookId)
        {
            var list = GetSearchCash();
            var book = list.FirstOrDefault(b => b.Id == bookId);
            if (book != null) list.Remove(book);
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
        /// Получение кеша всех книг
        /// </summary>
        private List<DtoSearchresultBook> GetBooksCash()
        {
            object obj = _cache.Get(KeyId);
            return obj as List<DtoSearchresultBook>;
        }

        /// <summary>
        /// Получение кеша комбо фильтров
        /// </summary>
        private BooksComboFilters GetFiltersCash()
        {
            object obj = _cache.Get(FilterId);
            return obj as BooksComboFilters;
        }

        /// <summary>
        /// Получения кеша отфильтрованых книг
        /// </summary>
        private List<DtoSearchresultBook> GetSearchCash()
        {
            object obj = _cache.Get(SearchId);
            return obj as List<DtoSearchresultBook>;
        }
    }
}
