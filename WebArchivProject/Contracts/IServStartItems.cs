using WebArchivProject.Models.DTO;

namespace WebArchivProject.Contracts
{
    public interface IServStartItems
    {
        DtoStartItem StartItem { get; }

        void InitStartItemCash();
        void UpdateStartItem(DtoStartItem dtoStartItem);
    }
}
