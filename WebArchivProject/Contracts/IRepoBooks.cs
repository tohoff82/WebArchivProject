using System.Collections.Generic;
using System.Threading.Tasks;

using WebArchivProject.Models.ArchivDb;

namespace WebArchivProject.Contracts
{
    /// <summary>
    /// контракт репозитория книг/публикаций
    /// </summary>
    public interface IRepoBooks
    {
        Task AddBookAsync(Book book);
        Task DeleteBookAsync(Book book);
        Task<Book> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> ToListAsync();
    }
}
