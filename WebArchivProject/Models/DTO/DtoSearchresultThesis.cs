using System.Collections.Generic;

namespace WebArchivProject.Models.DTO
{
    /// <summary>
    /// Дто объект визуализации найденных тезисов
    /// </summary>
    public class DtoSearchresultThesis
    {
        public int Id { get; set; }
        public List<string> Authors { get; set; }
        public string Name { get; set; }
        public string ConferenceName { get; set; }
        public string Year { get; set; }
        public string Location { get; set; }
        public string PagesInterval { get; set; }
        public int PagesCount { get; set; }
    }
}
