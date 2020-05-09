using System.Collections.Generic;
using System.Threading.Tasks;

using WebArchivProject.Models.DTO;
using WebArchivProject.Models.SearchFilters;
using WebArchivProject.Models.VO;

namespace WebArchivProject.Contracts
{
    public interface IServTheses
    {
        Task AddToDbAsync(DtoThesis dtoThesis);
        Task DeleteFromDbAsync(int thesisId);
        ThesesComboFilters GetThesesComboFilters();
        Paginator<DtoSearchresultThesis> GetPaginatorResultModal(ThesesSearchFilter filter);
        Paginator<DtoSearchresultThesis> GetPaginationResult(int pageNumber, int pageSize, string target);
        Paginator<DtoSearchresultThesis> GetThesesSearchPaginator(int pageNumber, int pageSize, string target);
        List<DtoSearchresultThesis> GetSearchCash();
    }
}
