using System.Collections.Generic;
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
        Task UpdateThesisAsync(Thesis thesis);
        Task DeleteThesisAsync(Thesis thesis);
        Task<Thesis> GetThesisByIdAsync(int id);
        Task<IEnumerable<Thesis>> ToListAsync();
        Task<IEnumerable<Thesis>> FilteredThesesToListAsync(string year, string name, string pages);
    }
}
