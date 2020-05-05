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
    /// Имплементация конратка репозитория тезисов
    /// </summary>
    class RepoTheses : IRepoTheses
    {
        private readonly ArchivContext _context;

        public RepoTheses(ArchivContext context)
        {
            _context = context;
        }

        public async Task AddThesisAsync(Thesis thesis)
        {
            await _context.Theses.AddAsync(thesis);
            await _context.SaveChangesAsync();
        }
    }
}
