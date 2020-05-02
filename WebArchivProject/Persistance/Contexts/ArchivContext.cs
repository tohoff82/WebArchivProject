using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebArchivProject.Persistance.Contexts
{
    public class ArchivContext : DbContext
    {
        public ArchivContext(DbContextOptions<ArchivContext> options)
            :base(options) { }
    }
}
