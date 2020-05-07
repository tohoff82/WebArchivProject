using System.Collections.Generic;

namespace WebArchivProject.Models.DTO
{
    /// <summary>
    /// Дто объект визуализации найденных книг
    /// </summary>
    public class DtoSearchresultBook
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string AuthorFirst { get; set; }
        public List<string> AuthorsNext { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public int MaxPageCount { get; set; }
        public string Type { get; set; }
        public string IssuerLine { get; set; }
    }
}
