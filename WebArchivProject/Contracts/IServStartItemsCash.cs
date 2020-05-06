using WebArchivProject.Models.DTO;

namespace WebArchivProject.Contracts
{
    public interface IServStartItemsCash
    {
        DtoStartItem StartItem { get; }

        void InitStartItemCash();
        void UpdateStartItem(DtoStartItem dtoStartItem);
    }
}
