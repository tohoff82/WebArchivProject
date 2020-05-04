using System.Threading.Tasks;

using WebArchivProject.Models.DTO;

namespace WebArchivProject.Contracts
{
    public interface IAuthManager
    {
        Task<DtoInterlayerIdentity> LoginAsync(DtoFormLoginUser loginUser);
        Task<DtoInterlayerIdentity> RegisterAsync(DtoFormRegisterUser registerUser);
    }
}
