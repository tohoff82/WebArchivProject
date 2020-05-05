using System.Threading.Tasks;

using WebArchivProject.Models.DTO;

namespace WebArchivProject.Contracts
{
    public interface IServTheses
    {
        Task AddToDbAsync(DtoThesis dtoThesis);
    }
}
