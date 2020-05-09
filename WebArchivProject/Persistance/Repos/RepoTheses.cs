using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Persistance.Contexts;

using static WebArchivProject.Helper.StringConstant;

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
        /// Фильтрация тезисов из БД
        /// </summary>
        /// <param name="year"></param>
        /// <param name="name"></param>
        /// <param name="pages"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Thesis>> FilteredThesesToListAsync(string year, string name, string pages)
        {
            if (year == DEFAULT_FILTER && name == DEFAULT_FILTER && pages == DEFAULT_FILTER)
                return await ToListAsync();
            if (year == DEFAULT_FILTER && name == DEFAULT_FILTER && pages != DEFAULT_FILTER)
                return await _context.Theses.AsNoTracking().Where(p => p.PagesInterval == pages).ToListAsync();
            if (year == DEFAULT_FILTER && name != DEFAULT_FILTER && pages == DEFAULT_FILTER)
                return await _context.Theses.AsNoTracking().Where(p => p.Name == name).ToListAsync();
            if (year != DEFAULT_FILTER && name == DEFAULT_FILTER && pages == DEFAULT_FILTER)
                return await _context.Theses.AsNoTracking().Where(p => p.Year == year).ToListAsync();
            if (year == DEFAULT_FILTER && name != DEFAULT_FILTER && pages != DEFAULT_FILTER)
                return await _context.Theses.AsNoTracking().Where(p => p.PagesInterval == pages && p.Name == name).ToListAsync();
            if (year != DEFAULT_FILTER && name == DEFAULT_FILTER && pages != DEFAULT_FILTER)
                return await _context.Theses.AsNoTracking().Where(p => p.Year == year && p.PagesInterval == pages).ToListAsync();
            if (year != DEFAULT_FILTER && name != DEFAULT_FILTER && pages == DEFAULT_FILTER)
                return await _context.Theses.AsNoTracking().Where(p => p.Year == year && p.Name == name).ToListAsync();
            else return await _context.Theses.AsNoTracking().Where(p => p.Year == year && p.Name == name && p.PagesInterval == pages).ToListAsync();
        }

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
