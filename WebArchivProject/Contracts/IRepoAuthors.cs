using System.Collections.Generic;
using System.Threading.Tasks;

using WebArchivProject.Models.ArchivDb;

namespace WebArchivProject.Contracts
{
    /// <summary>
    /// контракт рпозитория авторов
    /// </summary>
    public interface IRepoAuthors
    {
        Task AddAuthorAsync(Author author);
        Task AddAuthorsRangeAsync(List<Author> authors);
        Task UpdateAuthorsRangeAsync(List<Author> authors);
        Task DeleteAuthorsRangeAsync(List<Author> authors);
        Task<List<Author>> GetAuthorsByExtIdAsync(string id);
        Task<IEnumerable<Author>> ToListAsync();
    }
}
