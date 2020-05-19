using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebArchivProject.Models.DTO
{
    public class DtoThesisEdit
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Рік")]
        public string Year { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string ConferenceName { get; set; }
        public string DatesInterval { get; set; }
        public string PagesInterval { get; set; }

        public List<DtoAuthorEdit> Authors { get; set; }
    }
}
