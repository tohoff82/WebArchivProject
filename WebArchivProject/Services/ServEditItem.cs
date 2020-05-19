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
        private readonly IServPosts _servPosts;
        private readonly IServTheses _servTheses;
        private readonly IRepoBooks _repoBooks;
        private readonly IRepoPosts _repoPosts;
        private readonly IRepoTheses _repoTheses;
        private readonly IRepoAuthors _repoAuthors;

        public ServEditItem(
            IMapper mapper,
            IServBooks servBooks,
            IServPosts servPosts,
            IServTheses servTheses,
            IRepoBooks repoBooks,
            IRepoPosts repoPosts,
            IRepoTheses repoTheses,
            IRepoAuthors repoAuthors)
        {
            _mapper = mapper;
            _servBooks = servBooks;
            _servPosts = servPosts;
            _servTheses = servTheses;
            _repoBooks = repoBooks;
            _repoPosts = repoPosts;
            _repoTheses = repoTheses;
            _repoAuthors = repoAuthors;
        }

        /// <summary>
        /// Логика редактирования книги
        /// </summary>
        /// <param name="bookEdit"></param>
        public async Task EditBookAsync(DtoBookEdit bookEdit)
        {
            var book = await _repoBooks.GetBookByIdAsync(bookEdit.Id);
            var authors = await _repoAuthors.GetAuthorsByExtIdAsync(book.AuthorExternalId);

            _mapper.Map(bookEdit, book);
            var mappedAuthors = authors.CustomMap(bookEdit.Authors);

            await _repoAuthors.UpdateAuthorsRangeAsync(mappedAuthors);
            await _repoBooks.UpdateBookAsync(book);

            await _servBooks.UpdateBooksCashAsync();
            await _servBooks.UpdateBooksFiltersCashAsync();
        }

        /// <summary>
        /// Логика редактирования поста
        /// </summary>
        /// <param name="postEdit"></param>
        public async Task EditPostAsync(DtoPostEdit postEdit)
        {
            var post = await _repoPosts.GetPostByIdAsync(postEdit.Id);
            var authors = await _repoAuthors.GetAuthorsByExtIdAsync(post.AuthorExternalId);

            _mapper.Map(postEdit, post);
            var mappedAuthors = authors.CustomMap(postEdit.Authors);

            await _repoAuthors.UpdateAuthorsRangeAsync(mappedAuthors);
            await _repoPosts.UpdatePostAsync(post);

            await _servPosts.UpdatePostsCashAsync();
            await _servPosts.UpdatePostsFiltersCashAsync();
        }

        /// <summary>
        /// Логика редактирования тезиса
        /// </summary>
        /// <param name="thesisEdit"></param>
        public async Task EditThesisAsync(DtoThesisEdit thesisEdit)
        {
            var thesis = await _repoTheses.GetThesisByIdAsync(thesisEdit.Id);
            var authors = await _repoAuthors.GetAuthorsByExtIdAsync(thesis.AuthorExternalId);

            _mapper.Map(thesisEdit, thesis);
            var mappedAuthors = authors.CustomMap(thesisEdit.Authors);

            await _repoAuthors.UpdateAuthorsRangeAsync(mappedAuthors);
            await _repoTheses.UpdateThesisAsync(thesis);

            await _servTheses.UpdateThesesCashAsync();
            await _servTheses.UpdateThesesFiltersCashAsync();
        }
    }
}
