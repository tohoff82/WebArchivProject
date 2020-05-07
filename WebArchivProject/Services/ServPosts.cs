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
using WebArchivProject.Models.SearchFilters;
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

        private string FilterId => string
            .Format("Posts_Filter_{0}", _userSession.User.Id);

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

            await UpdatePostsCashAsync();
        }

        public async Task DeleteFromDbAsync(int postId)
        {
            var post = await _repoPosts.GetPostByIdAsync(postId);
            var authors = await _repoAuthors.GetAuthorsByExtIdAsync(post.AuthorExternalId);

            await _repoPosts.DeletePostAsync(post);
            await _repoAuthors.DeleteAuthorsRangeAsync(authors);

            await UpdatePostsCashAsync();
        }

        public Paginator<DtoSearchresultPost> GetPaginationResult(int pageNumber, int pageSize)
        {
            if (GetPostsCash() == null) UpdatePostsCashAsync().GetAwaiter().GetResult();
            return GetPostsPaginator(pageNumber, pageSize);
        }

        public PostsSearchFilter GetPostsSearchFilter()
        {
            if (GetFilterCash() == null) UpdatePostsFilterCashAsync().GetAwaiter().GetResult();
            return GetFilterCash();
        }

        private Paginator<DtoSearchresultPost> GetPostsPaginator(int pageNumber, int pageSize)
            => Paginator<DtoSearchresultPost>.ToList(GetPostsCash(), pageNumber, pageSize);

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

        private async Task UpdatePostsFilterCashAsync()
        {
            var posts = await _repoPosts.ToListAsync();
            var authors = await _repoAuthors.ToListAsync();

            _cache.Remove(FilterId);

            _cache.Set(FilterId,
            new PostsSearchFilter
            {
                Years = posts.OrderBy(b => b.Year)
                    .Select(b => b.Year).ToList(),
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

        private List<DtoSearchresultPost> GetPostsCash()
        {
            object obj = _cache.Get(KeyId);
            return obj as List<DtoSearchresultPost>;
        }

        private PostsSearchFilter GetFilterCash()
        {
            object obj = _cache.Get(FilterId);
            return obj as PostsSearchFilter;
        }
    }
}
