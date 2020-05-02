using System.Collections.Generic;

namespace WebArchivProject.Models.ArchivDb
{
    public class Post
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Year { get; set; }

        public string TomName { get; set; }
        public string Magazine { get; set; }
        public string MagazineNumber { get; set; }
        public string PagesInterval { get; set; }

        public List<Author> AuthorsList { get; set; }
    }
}
