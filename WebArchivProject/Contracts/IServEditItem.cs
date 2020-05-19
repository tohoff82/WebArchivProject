using System.Threading.Tasks;

using WebArchivProject.Models.DTO;

namespace WebArchivProject.Contracts
{
    public interface IServEditItem
    {
        Task EditBookAsync(DtoBookEdit bookEdit);
        Task EditPostAsync(DtoPostEdit postEdit);
        Task EditThesisAsync(DtoThesisEdit thesisEdit);
    }
}
