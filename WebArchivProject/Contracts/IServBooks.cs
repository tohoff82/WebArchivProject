using System.Threading.Tasks;

using WebArchivProject.Models.DTO;
using WebArchivProject.Models.VO;

namespace WebArchivProject.Contracts
{
    public interface IServBooks
    {
        Task AddToDbAsync(DtoBook dtoBook);
        Paginator<DtoSearchresultBook> GetPaginationResult(int pageNumber, int pageSize);
    }
}
