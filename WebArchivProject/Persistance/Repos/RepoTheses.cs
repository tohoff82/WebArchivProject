using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Persistance.Contexts;

namespace WebArchivProject.Persistance.Repos
{
    /// <summary>
    /// Имплементация конратка репозитория тезисов
    /// </summary>
    class RepoTheses : IRepoTheses
    {
        private readonly ArchivContext _context;

        public RepoTheses(ArchivContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение коллекции тезисов из БД
        /// </summary>
        public async Task<IEnumerable<Thesis>> ToListAsync()
            => await _context.Theses.AsNoTracking().ToListAsync();

        /// <summary>
        /// Получения тезиса из БД по айдишнику
        /// </summary>
        public async Task<Thesis> GetThesisByIdAsync(int id)
            => await _context.Theses.AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

        /// <summary>
        /// Добавление тезиса в БД
        /// </summary>
        public async Task AddThesisAsync(Thesis thesis)
        {
            await _context.Theses.AddAsync(thesis);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаление тезиса из БД
        /// </summary>
        public Task DeleteThesisAsync(Thesis thesis)
        {
            _context.Theses.Remove(thesis);
            return _context.SaveChangesAsync();
        }
    }
}
