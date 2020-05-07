﻿using System.Threading.Tasks;

using WebArchivProject.Models.DTO;
using WebArchivProject.Models.SearchFilters;
using WebArchivProject.Models.VO;

namespace WebArchivProject.Contracts
{
    public interface IServBooks
    {
        Task AddToDbAsync(DtoBook dtoBook);
        Task DeleteFromDbAsync(int bookId);
        BooksSearchFilter GetBooksSearchFilter();
        Paginator<DtoSearchresultBook> GetPaginationResult(int pageNumber, int pageSize);
    }
}
