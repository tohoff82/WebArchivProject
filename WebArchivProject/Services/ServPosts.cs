using AutoMapper;
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

            await UpdatePostsCash();
        }

        public Paginator<DtoSearchresultPost> GetPaginationResult(int pageNumber, int pageSize)
        {
            if (GetPostsCash() == null) UpdatePostsCash().GetAwaiter().GetResult();

            return GetPostsPaginator(pageNumber, pageSize);
        }

        private Paginator<DtoSearchresultPost> GetPostsPaginator(int pageNumber, int pageSize)
            => Paginator<DtoSearchresultPost>.ToList(GetPostsCash(), pageNumber, pageSize);

        private async Task UpdatePostsCash()
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

        private List<DtoSearchresultPost> GetPostsCash()
        {
            object obj = _cache.Get(KeyId);
            return obj as List<DtoSearchresultPost>;
        }
    }
}
