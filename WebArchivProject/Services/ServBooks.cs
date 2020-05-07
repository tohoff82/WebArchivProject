﻿using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;
using WebArchivProject.Models;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Models.DTO;
using WebArchivProject.Models.VO;

namespace WebArchivProject.Services
{
    class ServBooks : IServBooks
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IRepoBooks _repoBooks;
        private readonly IRepoAuthors _repoAuthors;
        private readonly IServUserSession _userSession;

        private string KeyId => string
            .Format("Books_{0}", _userSession.User.Id);

        private SessionUser User
            => _userSession.User;

        public ServBooks(
            IMapper mapper,
            IMemoryCache cache,
            IRepoBooks repoBooks,
            IRepoAuthors repoAuthors,
            IServUserSession userSession)
        {
            _mapper = mapper;
            _cache = cache;
            _repoBooks = repoBooks;
            _repoAuthors = repoAuthors;
            _userSession = userSession;
        }

        public async Task AddToDbAsync(DtoBook dtoBook)
        {
            string guid = Guid.NewGuid().ToString();

            var book = _mapper.Map<Book>(dtoBook);
            book.AuthorExternalId = guid;
            book.OwnerId = User.Id;
            await _repoBooks.AddBookAsync(book);

            var authors = _mapper.Map<List<Author>>(dtoBook.Authors);
            await _repoAuthors.AddAuthorsRangeAsync(authors.With(guid));

            await UpdateBooksCash();
        }

        public async Task DeleteFromDbAsync(int bookId)
        {
            var book = await _repoBooks.GetBookByIdAsync(bookId);
            var authors = await _repoAuthors.GetAuthorsByExtIdAsync(book.AuthorExternalId);

            await _repoBooks.DeleteBookAsync(book);
            await _repoAuthors.DeleteAuthorsRangeAsync(authors);

            await UpdateBooksCash();
        }

        public Paginator<DtoSearchresultBook> GetPaginationResult(int pageNumber, int pageSize)
        {
            if (GetBooksCash() == null) UpdateBooksCash().GetAwaiter().GetResult();
            
            return GetBooksPaginator(pageNumber, pageSize);
        }

        private Paginator<DtoSearchresultBook> GetBooksPaginator(int pageNumber, int pageSize)
            => Paginator<DtoSearchresultBook>.ToList(GetBooksCash(), pageNumber, pageSize);

        private async Task UpdateBooksCash()
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

        private List<DtoSearchresultBook> GetBooksCash()
        {
            object obj = _cache.Get(KeyId);
            return obj as List<DtoSearchresultBook>;
        }
    }
}
