namespace WebArchivProject.Models.ArchivDb
{
    /// <summary>
    /// Модель БД, представляющая книгу/методичку
    /// </summary>
    public class Book
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }

        public string Name { get; set; }
        public string Year { get; set; }

        public string Type { get; set; }
        public string City { get; set; }
        public string Issuer { get; set; }
        public int MaxPageCount { get; set; }

        public string AuthorExternalId { get; set; }
    }
}
