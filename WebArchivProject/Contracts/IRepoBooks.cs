using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
