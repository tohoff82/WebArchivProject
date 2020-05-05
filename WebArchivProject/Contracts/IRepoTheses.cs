using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebArchivProject.Models.ArchivDb;

namespace WebArchivProject.Contracts
{
    /// <summary>
    /// Контракт репозитория тезисов
    /// </summary>
    public interface IRepoTheses
    {
        Task AddThesisAsync(Thesis thesis);
    }
}
