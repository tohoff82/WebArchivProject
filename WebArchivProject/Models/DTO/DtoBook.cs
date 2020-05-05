using System.Collections.Generic;

namespace WebArchivProject.Models.DTO
{
    /// <summary>
    /// ДТО Объект книги
    /// </summary>
    public class DtoBook
    {
        public string Name { get; set; }
        public string Year { get; set; }

        public string Type { get; set; }
        public string City { get; set; }
        public string Issuer { get; set; }
        public int MaxPageCount { get; set; }

        public List<DtoAuthor> Authors { get; set; }
    }
}
