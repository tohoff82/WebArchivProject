using System.Threading.Tasks;

using WebArchivProject.Models.DTO;

namespace WebArchivProject.Contracts
{
    public interface IAuthManager
    {
        Task<DtoInterlayerLogin> LoginAsync(DtoFormLoginUser loginUser);
        Task RegisterAsync(DtoFormRegisterUser registerUser);
    }
}
