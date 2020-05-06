using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;
using WebArchivProject.Models;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Models.DTO;
using WebArchivProject.Models.VO;

namespace WebArchivProject.Services
{
    class ServPosts : IServPosts
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IRepoPosts _repoPosts;
        private readonly IRepoAuthors _repoAuthors;
        private readonly IServUserSession _userSession;

        private string KeyId => string
            .Format("Posts_{0}", _userSession.User.Id);

        private SessionUser User
            => _userSession.User;

        public ServPosts(
            IMapper mapper,
            IMemoryCache cache,
            IRepoPosts repoPosts,
            IRepoAuthors repoAuthors,
            IServUserSession userSession)
        {
            _mapper = mapper;
            _cache = cache;
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

        public Paginator<DtoSearchresultPost> GetPaginationResult(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
