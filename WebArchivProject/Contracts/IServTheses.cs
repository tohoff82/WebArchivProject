using System.Threading.Tasks;

using WebArchivProject.Models.DTO;
using WebArchivProject.Models.VO;

namespace WebArchivProject.Contracts
{
    public interface IServTheses
    {
        Task AddToDbAsync(DtoThesis dtoThesis);
        Paginator<DtoSearchresultThesis> GetPaginationResult(int pageNumber, int pageSize);
    }
}
