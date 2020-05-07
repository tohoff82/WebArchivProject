namespace WebArchivProject.Contracts
{
    public interface IPaginator
    {
        int CurrentPage { get; }
        int TotalPages { get; }
        int PageSize { get; }
        int TotalCount { get; }

        bool HasPrevious { get; }
        bool HasNext { get; }

        // custom properties
        string ForTable { get; set; }
        string ForContainer { get; set; }
        string Size { get; set; }
    }
}
