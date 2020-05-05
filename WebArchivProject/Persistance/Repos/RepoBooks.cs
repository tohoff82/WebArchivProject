﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebArchivProject.Contracts;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Persistance.Contexts;

namespace WebArchivProject.Persistance.Repos
{
    /// <summary>
    /// имплементация контракта репозитория книг/публикаций
    /// </summary>
    class RepoBooks : IRepoBooks
    {
        private readonly ArchivContext _context;

        public RepoBooks(ArchivContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавление книги в БД
        /// </summary>
        /// <param name="book"></param>
        public async Task AddBookAsync(Book book )
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }
    }
}
