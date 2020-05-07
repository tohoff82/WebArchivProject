using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebArchivProject.Models.ArchivDb;

namespace WebArchivProject.Contracts
{
    /// <summary>
    /// контракт репозитория постов
    /// </summary>
    public interface IRepoPosts
    {
        Task AddPostAsync(Post post);
        Task DeletePostAsync(Post post);
        Task<Post> GetPostByIdAsync(int id);
        Task<IEnumerable<Post>> ToListAsync();
    }
}
