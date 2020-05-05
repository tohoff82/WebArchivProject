using System.Collections.Generic;

namespace WebArchivProject.Models.DTO
{
    public class DtoPost
    {
        public string Name { get; set; }
        public string Year { get; set; }

        public string TomName { get; set; }
        public string Magazine { get; set; }
        public string MagazineNumber { get; set; }
        public string PagesInterval { get; set; }

        public List<DtoAuthor> Authors { get; set; }
    }
}
