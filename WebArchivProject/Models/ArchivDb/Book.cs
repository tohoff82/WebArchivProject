using System.Collections.Generic;

namespace WebArchivProject.Models.ArchivDb
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Year { get; set; }

        public string Type { get; set; }
        public string City { get; set; }
        public string Issuer { get; set; }
        public int MaxPageCount { get; set; }

        public List<Author> Authors { get; set; }
    }
}
