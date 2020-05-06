using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<List<Author>> GetAuthorsByExtIdAsync(string id);
    }
}
