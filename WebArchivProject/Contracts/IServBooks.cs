using System.Collections.Generic;
using System.Threading.Tasks;

using WebArchivProject.Models.DTO;
using WebArchivProject.Models.SearchFilters;
using WebArchivProject.Models.VO;

namespace WebArchivProject.Contracts
{
    public interface IServBooks
    {
        Task AddToDbAsync(DtoBook dtoBook);
        Task DeleteFromDbAsync(int bookId);
        BooksComboFilters GetBooksComboFilters();
        Paginator<DtoSearchresultBook> GetPaginatorResultModal(BooksSearchFilter filter);
        Paginator<DtoSearchresultBook> GetPaginationResult(int pageNumber, int pageSize, string target);
        Paginator<DtoSearchresultBook> GetBooksSearchPaginator(int pageNumber, int pageSize, string target);
        List<DtoSearchresultBook> GetSearchCash();
    }
}
