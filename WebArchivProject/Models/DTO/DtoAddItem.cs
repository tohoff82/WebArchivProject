namespace WebArchivProject.Models.DTO
{
    /// <summary>
    /// ДТО Объект добавляемой еденицы в БД
    /// </summary>
    public class DtoAddItem
    {
        public string CreationId { get; set; }
        public DtoStartItem StartItem { get; set; }
        public DtoFormBook SubItem { get; set; }
    }
}
