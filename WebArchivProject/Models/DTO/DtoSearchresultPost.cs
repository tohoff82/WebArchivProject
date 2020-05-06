using System.Collections.Generic;

namespace WebArchivProject.Models.DTO
{
    /// <summary>
    /// Дто объект визуализации найденных постов
    /// </summary>
    public class DtoSearchresultPost
    {
        public int Id { get; set; }
        public List<string> Authors { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public string TomName { get; set; }
        public string Magazine { get; set; }
        public string PagesInterval { get; set; }
        public int PagesCount { get; set; }
    }
}
