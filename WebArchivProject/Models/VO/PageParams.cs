namespace WebArchivProject.Models.VO
{
    public class PageParams
    {
        const int _maxPageSize = 10;
        private int _pageSize = 3;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
            }
        }
    }
}
