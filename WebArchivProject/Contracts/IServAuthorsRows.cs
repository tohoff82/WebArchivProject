using System.Collections.Generic;

using WebArchivProject.Models.DTO;

namespace WebArchivProject.Contracts
{
    public interface IServAuthorsRows
    {
        SortedDictionary<byte, DtoAuthor> AuthorsRows { get; }

        void InitAuthorsRowsCash();
        void HandleAddRow(List<DtoAuthor> authors);
        void HandleUpdateRow(List<DtoAuthor> authors);
    }
}
