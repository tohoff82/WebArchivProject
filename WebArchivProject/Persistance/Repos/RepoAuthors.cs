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
    /// Имплементация контракта репозитория авторов
    /// </summary>
    class RepoAuthors : IRepoAuthors
    {
        private readonly ArchivContext _context;

        public RepoAuthors(ArchivContext context)
        {
            _context = context;
        }

        public async Task AddAuthorAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
        }

        public async Task AddAuthorsRangeAsync(List<Author> authors)
        {
            await _context.Authors.AddRangeAsync(authors);
            await _context.SaveChangesAsync();
        }
    }
}
