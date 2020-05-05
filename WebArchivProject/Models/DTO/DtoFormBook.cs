namespace WebArchivProject.Models.DTO
{
    /// <summary>
    /// дто объект типа добавляемого элемента (книга, пос. тезис) из формы ввода
    /// </summary>
    public class DtoFormBook
    {
        public string Type { get; set; }
        public string City { get; set; }
        public string Issuer { get; set; }
        public int MaxPageCount { get; set; }
    }
}
