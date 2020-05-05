using System.Threading.Tasks;

using WebArchivProject.Models.DTO;

namespace WebArchivProject.Contracts
{
    public interface IServPosts
    {
        Task AddToDbAsync(DtoPost dtoPost);
    }
}
