﻿using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<AppUser>> ToListAsync()
            => await _context.AppUsers.AsNoTracking().ToListAsync();

        public async Task<AppUser> GetAppUserByIdAsync(int id)
            => await _context.AppUsers.AsNoTracking().FirstOrDefaultAsync(u
                => u.Id == id);

        public async Task<AppUser> GetAppUserByEmailAsync(string email)
            => await _context.AppUsers.AsNoTracking().FirstOrDefaultAsync(u
                => u.Mail == email);

        public async Task<AppUser> GetAppUserByNameAsync(string name)
            => await _context.AppUsers.AsNoTracking().FirstOrDefaultAsync(u
                => u.Name == name);

        public async Task AddAsync(AppUser appUser)
        {
            await _context.AddAsync(appUser);
            await _context.SaveChangesAsync();
        }

        public Task UpdateUserAsync(AppUser appUser)
        {
            _context.AppUsers.Update(appUser);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAppUserAsync(AppUser appUser)
        {
            _context.AppUsers.Remove(appUser);
            return _context.SaveChangesAsync();
        }
    }
}
