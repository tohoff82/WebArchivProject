using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebArchivProject.Contracts;
using WebArchivProject.Persistance.Contexts;

namespace WebArchivProject.Persistance.Repos
{
    class RepoTheses : IRepoTheses
    {
        private readonly ArchivContext _context;

        public RepoTheses(ArchivContext context)
        {
            _context = context;
        }
    }
}
