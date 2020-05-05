using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;
using WebArchivProject.Models;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Services
{
    class ServBooks : IServBooks
    {
        private readonly IMapper _mapper;
        private readonly IRepoBooks _repoBooks;
        private readonly IRepoAuthors _repoAuthors;
        private readonly IServUserSession _userSession;

        private SessionUser User
            => _userSession.User;

        public ServBooks(
            IMapper mapper,
            IRepoBooks repoBooks,
            IRepoAuthors repoAuthors,
            IServUserSession userSession)
        {
            _mapper = mapper;
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
        }
    }
}
