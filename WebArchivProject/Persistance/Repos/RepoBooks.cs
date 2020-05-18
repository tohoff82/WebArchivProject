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
    /// имплементация контракта репозитория книг/публикаций
    /// </summary>
    class RepoBooks : IRepoBooks
    {
        private readonly ArchivContext _context;

        public RepoBooks(ArchivContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение коллекции книг из БД
        /// </summary>
        public async Task<IEnumerable<Book>> ToListAsync()
            => await _context.Books.AsNoTracking().ToListAsync();

        /// <summary>
        /// Получение книги из БД по ее айдишнику
        /// </summary>
        public async Task<Book> GetBookByIdAsync(int id)
            => await _context.Books.AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

        /// <summary>
        /// Фильтрация книг из БД
        /// </summary>
        /// <param name="year">год</param>
        /// <param name="name">название</param>
        /// <returns></returns>
        public async Task<IEnumerable<Book>> FilteredBooksToListAsync(string year, string name)
        {
            if (year == DEFAULT_FILTER && name == DEFAULT_FILTER) return await ToListAsync();
            if (year == DEFAULT_FILTER) return await _context.Books.AsNoTracking()
                    .Where(b => b.Name == name).ToListAsync();
            if (name == DEFAULT_FILTER) return await _context.Books.AsNoTracking()
                    .Where(b => b.Year == year).ToListAsync();
            else return await _context.Books.AsNoTracking().Where(b
                => b.Name == name && b.Year == year).ToListAsync();
        }

        /// <summary>
        /// Добавление книги в БД
        /// </summary>
        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновление книги
        /// </summary>
        /// <param name="book"></param>
        public Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            return _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаление книги из БД
        /// </summary>
        public Task DeleteBookAsync(Book book)
        {
            _context.Books.Remove(book);
            return _context.SaveChangesAsync();
        }
    }
}
