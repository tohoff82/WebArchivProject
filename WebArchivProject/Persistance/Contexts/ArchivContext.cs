﻿using Microsoft.EntityFrameworkCore;

using WebArchivProject.Models.ArchivDb;

namespace WebArchivProject.Persistance.Contexts
{
    /// <summary>
    /// Крнтекст базы данных
    /// </summary>
    public class ArchivContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Thesis> Theses { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Post> Posts { get; set; }

        public ArchivContext(DbContextOptions<ArchivContext> options)
            : base(options) { }
    }
}
