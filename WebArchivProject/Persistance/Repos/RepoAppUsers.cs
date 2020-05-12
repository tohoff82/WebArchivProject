using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Persistance.Contexts;

namespace WebArchivProject.Persistance.Repos
{
    class RepoAppUsers : IRepoAppUsers
    {
        private readonly ArchivContext _context;

        public RepoAppUsers(ArchivContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение списка пользователей из БД
        /// </summary>
        public async Task<IEnumerable<AppUser>> ToListAsync()
            => await _context.AppUsers.AsNoTracking().ToListAsync();

        /// <summary>
        /// Получение пользователя из БД по идентификатору
        /// </summary>
        public async Task<AppUser> GetAppUserByIdAsync(int id)
            => await _context.AppUsers.AsNoTracking().FirstOrDefaultAsync(u
                => u.Id == id);

        /// <summary>
        /// Получение пользователя из БД по емейлу
        /// </summary>
        public async Task<AppUser> GetAppUserByEmailAsync(string email)
            => await _context.AppUsers.AsNoTracking().FirstOrDefaultAsync(u
                => u.Mail == email);

        /// <summary>
        /// Добавление пользователя в БД
        /// </summary>
        public async Task AddAsync(AppUser appUser)
        {
            await _context.AddAsync(appUser);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновление пользователя в БД
        /// </summary>
        public Task UpdateUserAsync(AppUser appUser)
        {
            _context.AppUsers.Update(appUser);
            return _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаление пользователя изБД
        /// </summary>
        public Task DeleteAppUserAsync(AppUser appUser)
        {
            _context.AppUsers.Remove(appUser);
            return _context.SaveChangesAsync();
        }
    }
}
