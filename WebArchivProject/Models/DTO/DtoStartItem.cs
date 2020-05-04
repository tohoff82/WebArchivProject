using System.Collections.Generic;

namespace WebArchivProject.Models.DTO
{
    /// <summary>
    /// ДТО Объект стартовой карточки
    /// </summary>
    public class DtoStartItem
    {
        public string Name { get; set; }
        public string Year { get; set; }
        public string ItemType { get; set; }
        public List<DtoAuthor> Authors { get; set; }
    }
}
