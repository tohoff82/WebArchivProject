namespace WebArchivProject.Models.ArchivDb
{
    /// <summary>
    /// Модель БД представляющая автора
    /// </summary>
    public class Author
    {
        public int Id { get; set; }

        public string NameRu { get; set; }
        public string NameUa { get; set; }
        public string NameEn { get; set; }
        public bool IsFirst { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int ThesisId { get; set; }
        public Thesis Thesis { get; set; }
    }
}
