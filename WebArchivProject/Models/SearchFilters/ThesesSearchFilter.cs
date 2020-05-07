using System.Collections.Generic;

namespace WebArchivProject.Models.SearchFilters
{
    public class ThesesSearchFilter
    {
        public List<string> Years { get; set; }
        public List<string> Names { get; set; }
        public List<string> Authors { get; set; }
        public List<string> Pages { get; set; }
    }
}
