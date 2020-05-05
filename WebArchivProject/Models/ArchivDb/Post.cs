namespace WebArchivProject.Models.ArchivDb
{
    /// <summary>
    /// Модель БД, представляющая пост
    /// </summary>
    public class Post
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }

        public string Name { get; set; }
        public string Year { get; set; }

        public string TomName { get; set; }
        public string Magazine { get; set; }
        public string MagazineNumber { get; set; }
        public string PagesInterval { get; set; }

        public string AuthorExternalId { get; set; }
    }
}
