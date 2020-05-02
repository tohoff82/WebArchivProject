using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebArchivProject.Contracts;
using WebArchivProject.Persistance.Contexts;

namespace WebArchivProject.Persistance.Repos
{
    class RepoPosts : IRepoPosts
    {
        private readonly ArchivContext _context;

        public RepoPosts(ArchivContext context)
        {
            _context = context;
        }
    }
}
