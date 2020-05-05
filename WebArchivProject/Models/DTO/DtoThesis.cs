using System.Collections.Generic;

namespace WebArchivProject.Models.DTO
{
    public class DtoThesis
    {
        public string Name { get; set; }
        public string Year { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string ConferenceName { get; set; }
        public string DatesInterval { get; set; }
        public string PagesInterval { get; set; }

        public List<DtoAuthor> Authors { get; set; }
    }
}
