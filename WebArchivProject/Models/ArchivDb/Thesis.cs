namespace WebArchivProject.Models.ArchivDb
{
    /// <summary>
    /// Модель БД представляющая тезис
    /// </summary>
    public class Thesis
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }

        public string Name { get; set; }
        public string Year { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string ConferenceName { get; set; }
        public string DatesInterval { get; set; }
        public string PagesInterval { get; set; }

        public string AuthorExternalId { get; set; }
    }
}
