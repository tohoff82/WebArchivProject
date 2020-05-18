using AutoMapper;

using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Services
{
    public class ServEditItem : IServEditItem
    {
        private readonly IMapper _mapper;
        private readonly IServBooks _servBooks;
        private readonly IRepoBooks _repoBooks;
        private readonly IRepoPosts _repoPosts;
        private readonly IRepoTheses _repoTheses;
        private readonly IRepoAuthors _repoAuthors;

        public ServEditItem(
            IMapper mapper,
            IServBooks servBooks,
            IRepoBooks repoBooks,
            IRepoPosts repoPosts,
            IRepoTheses repoTheses,
            IRepoAuthors repoAuthors)
        {
            _mapper = mapper;
            _servBooks = servBooks;
            _repoBooks = repoBooks;
            _repoPosts = repoPosts;
            _repoTheses = repoTheses;
            _repoAuthors = repoAuthors;
        }

        public async Task EditBookAsync(DtoBookEdit bookEdit)
        {
            var book = await _repoBooks.GetBookByIdAsync(bookEdit.Id);
            var authors = await _repoAuthors.GetAuthorsByExtIdAsync(book.AuthorExternalId);

            _mapper.Map(bookEdit, book);
            var mappedAuthors = authors.CustomMap(bookEdit.Authors);

            await _repoAuthors.UpdateAuthorsRangeAsync(mappedAuthors);
            await _repoBooks.UpdateBookAsync(book);
            await _servBooks.UpdateBooksCashAsync();
        }
    }
}
