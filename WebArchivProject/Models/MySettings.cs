namespace WebArchivProject.Models
{
    public class MySettings
    {
        public PagerSettings PagerSettings { get; set; }
    }

    public class PagerSettings
    {
        public int ItemPerPage { get; set; }
    }
}
