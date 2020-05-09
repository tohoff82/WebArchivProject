namespace WebArchivProject.Contracts
{
    /// <summary>
    /// Контракт сервиса експорта
    /// </summary>
    public interface IServExport
    {
        public bool BooksExportDisabled { get; }
        public bool PostsExportDisabled { get; }
        public bool ThesesExportDisabled { get; }

        byte[] GetExelBooksBuffer();
        byte[] GetExelPostsBuffer();
        byte[] GetExelThesesBuffer();
    }
}
