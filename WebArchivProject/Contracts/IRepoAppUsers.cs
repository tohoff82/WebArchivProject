using System.Collections.Generic;
using System.Threading.Tasks;

using WebArchivProject.Models.ArchivDb;

namespace WebArchivProject.Contracts
{
    public interface IRepoAppUsers
    {
        Task<IEnumerable<AppUser>> ToListAsync();
        Task<AppUser> GetAppUserByIdAsync(int id);
        Task<AppUser> GetAppUserByEmailAsync(string email);
        Task AddAsync(AppUser appUser);
        Task UpdateUserAsync(AppUser appUser);
        Task DeleteAppUserAsync(AppUser appUser);
    }
}
