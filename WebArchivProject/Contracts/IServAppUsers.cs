using System.Threading.Tasks;

using WebArchivProject.Models.DTO;
using WebArchivProject.Models.VO;

namespace WebArchivProject.Contracts
{
    public interface IServAppUsers
    {
        Task<Paginator<DtoAppUserView>> GetPaginatorAsync(int page = 1);
    }
}
