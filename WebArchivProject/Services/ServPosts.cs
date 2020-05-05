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
    class ServPosts : IServPosts
    {
        private readonly IMapper _mapper;
        private readonly IRepoPosts _repoPosts;
        private readonly IRepoAuthors _repoAuthors;
        private readonly IServUserSession _userSession;

        private SessionUser User
            => _userSession.User;

        public ServPosts(
            IMapper mapper,
            IRepoPosts repoPosts,
            IRepoAuthors repoAuthors,
            IServUserSession userSession)
        {
            _mapper = mapper;
            _repoPosts = repoPosts;
            _repoAuthors = repoAuthors;
            _userSession = userSession;
        }

        public async Task AddToDbAsync(DtoPost dtoPost)
        {
            string guid = Guid.NewGuid().ToString();

            var post = _mapper.Map<Post>(dtoPost);
            post.AuthorExternalId = guid;
            post.OwnerId = User.Id;
            await _repoPosts.AddPostAsync(post);

            var authors = _mapper.Map<List<Author>>(dtoPost.Authors);
            await _repoAuthors.AddAuthorsRangeAsync(authors.With(guid));
        }
    }
}
