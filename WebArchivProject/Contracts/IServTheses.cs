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
        ThesesSearchFilter GetThesesSearchFilter();
        Paginator<DtoSearchresultThesis> GetPaginationResult(int pageNumber, int pageSize);
    }
}
