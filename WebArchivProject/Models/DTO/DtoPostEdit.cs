using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebArchivProject.Models.DTO
{
    public class DtoPostEdit
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Рік")]
        public string Year { get; set; }

        public string TomName { get; set; }
        public string Magazine { get; set; }
        public string MagazineNumber { get; set; }
        public string PagesInterval { get; set; }

        public List<DtoAuthorEdit> Authors { get; set; }
    }
}
