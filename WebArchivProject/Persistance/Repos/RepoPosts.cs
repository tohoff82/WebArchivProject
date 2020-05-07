using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebArchivProject.Contracts;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Persistance.Contexts;

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
        /// Добавление поста в БД
        /// </summary>
        public async Task AddPostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
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
