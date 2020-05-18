using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebArchivProject.Models.DTO
{
    public class DtoBookEdit
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Рік")]
        public string Year { get; set; }

        public string Type { get; set; }
        public string City { get; set; }
        public string Issuer { get; set; }
        public int MaxPageCount { get; set; }

        //public string AuthorExternalId { get; set; }
        public List<DtoAuthorEdit> Authors { get; set; }
    }
}
