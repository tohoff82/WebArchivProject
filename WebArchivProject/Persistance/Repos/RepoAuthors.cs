using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebArchivProject.Contracts;
using WebArchivProject.Persistance.Contexts;

namespace WebArchivProject.Persistance.Repos
{
    class RepoAuthors : IRepoAuthors
    {
        private readonly ArchivContext _context;

        public RepoAuthors(ArchivContext context)
        {
            _context = context;
        }
    }
}
