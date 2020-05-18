using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Persistance.Contexts;

namespace WebArchivProject.Persistance.Repos
{
    /// <summary>
    /// Имплементация контракта репозитория авторов
    /// </summary>
    class RepoAuthors : IRepoAuthors
    {
        private readonly ArchivContext _context;

        public RepoAuthors(ArchivContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение объекта автора по внешнему айдишнику
        /// </summary>
        /// <param name="id">внешний айди</param>
        public Task<List<Author>> GetAuthorsByExtIdAsync(string id)
            => _context.Authors.AsNoTracking().Where(a
                => a.ExternalId == id).ToListAsync();

        /// <summary>
        /// Получение списка всех авторов
        /// </summary>
        public async Task<IEnumerable<Author>> ToListAsync()
            => await _context.Authors.AsNoTracking().ToListAsync();

        /// <summary>
        /// Добавление нового автора в БД
        /// </summary>
        public async Task AddAuthorAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Добавление пачки авторов в БД
        /// </summary>
        public async Task AddAuthorsRangeAsync(List<Author> authors)
        {
            await _context.Authors.AddRangeAsync(authors);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновление авторов
        /// </summary>
        /// <param name="authors"></param>
        public Task UpdateAuthorsRangeAsync(List<Author> authors)
        {
            _context.Authors.UpdateRange(authors);
            return _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаляем связанных авторов
        /// </summary>
        /// <param name="authors"></param>
        public Task DeleteAuthorsRangeAsync(List<Author> authors)
        {
            _context.Authors.RemoveRange(authors);
            return _context.SaveChangesAsync();
        }
    }
}
