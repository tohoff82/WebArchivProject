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
    /// имплементация контракта репозитория постов
    /// </summary>
    class RepoPosts : IRepoPosts
    {
        private readonly ArchivContext _context;

        public RepoPosts(ArchivContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение коллекции постов из БД
        /// </summary>
        public async Task<IEnumerable<Post>> ToListAsync()
            => await _context.Posts.AsNoTracking().ToListAsync();

        /// <summary>
        /// Получение поста из БД по айдишнику
        /// </summary>
        public async Task<Post> GetPostByIdAsync(int id)
            => await _context.Posts.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

        /// <summary>
        /// Фильтрация статей из БД
        /// </summary>
        /// <param name="year"></param>
        /// <param name="name"></param>
        /// <param name="magazine"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Post>> FilteredPostsToListAsync(string year, string name, string magazine)
        {
            if (year == DEFAULT_FILTER && name == DEFAULT_FILTER && magazine == DEFAULT_FILTER)
                return await ToListAsync();
            if (year == DEFAULT_FILTER && name == DEFAULT_FILTER && magazine != DEFAULT_FILTER)
                return await _context.Posts.AsNoTracking().Where(p => p.Magazine == magazine).ToListAsync();
            if (year == DEFAULT_FILTER && name != DEFAULT_FILTER && magazine == DEFAULT_FILTER)
                return await _context.Posts.AsNoTracking().Where(p => p.Name == name).ToListAsync();
            if (year != DEFAULT_FILTER && name == DEFAULT_FILTER && magazine == DEFAULT_FILTER)
                return await _context.Posts.AsNoTracking().Where(p => p.Year == year).ToListAsync();
            if (year == DEFAULT_FILTER && name != DEFAULT_FILTER && magazine != DEFAULT_FILTER)
                return await _context.Posts.AsNoTracking().Where(p => p.Magazine == magazine && p.Name == name).ToListAsync();
            if (year != DEFAULT_FILTER && name == DEFAULT_FILTER && magazine != DEFAULT_FILTER)
                return await _context.Posts.AsNoTracking().Where(p => p.Year == year && p.Magazine == magazine).ToListAsync();
            if (year != DEFAULT_FILTER && name != DEFAULT_FILTER && magazine == DEFAULT_FILTER)
                return await _context.Posts.AsNoTracking().Where(p => p.Year == year && p.Name == name).ToListAsync();
            else return await _context.Posts.AsNoTracking().Where(p => p.Year == year && p.Name == name && p.Magazine == magazine).ToListAsync();
        }

        /// <summary>
        /// Добавление поста в БД
        /// </summary>
        public async Task AddPostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновление поста в БД
        /// </summary>
        /// <param name="post"></param>
        public Task UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
            return _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаление поста из БД
        /// </summary>
        public Task DeletePostAsync(Post post)
        {
            _context.Posts.Remove(post);
            return _context.SaveChangesAsync();
        }
    }
}
