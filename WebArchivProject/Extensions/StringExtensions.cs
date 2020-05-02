namespace WebArchivProject.Extensions
{
    public static class StringExtensions
    {
        public static string ToNextId(this string id)
            => string.Concat("_del_row_", id);
    }
}
