using System.Threading.Tasks;

using WebArchivProject.Models.DTO;

namespace WebArchivProject.Contracts
{
    public interface IServBooks
    {
        Task AddToDbAsync(DtoBook dtoBook);
    }
}
