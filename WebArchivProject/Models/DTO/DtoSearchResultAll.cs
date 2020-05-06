using WebArchivProject.Models.VO;

namespace WebArchivProject.Models.DTO
{
    public class DtoSearchResultAll
    {
        public Paginator<DtoSearchresultBook> BooksPager { get; set; }
        public Paginator<DtoSearchresultPost> PostsPager { get; set; }
        public Paginator<DtoSearchresultThesis> ThesesPager { get; set; }
    }
}
