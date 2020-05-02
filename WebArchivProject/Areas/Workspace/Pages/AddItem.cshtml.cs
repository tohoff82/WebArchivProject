using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WebArchivProject.Contracts;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// Модель страницы добавления в БД
    /// </summary>
    public class AddItemModel : PageModel
    {
        /// <summary>
        /// сервис генерирующий криптографически строковый строковый идентификатор
        /// </summary>
        private readonly IServCryptografy _cryptografy;

        /// <summary>
        /// конструктор модели, принимающий вышеуказанный сервис
        /// </summary>
        /// <param name="cryptografy"></param>
        public AddItemModel(
            IServCryptografy cryptografy)
        {
            _cryptografy = cryptografy;
        }

        /// <summary>
        /// обработчик страницы по умолчанию
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// обработчик кнопки "Далее"
        /// </summary>
        /// <param name="startItem"></param>
        /// <returns>Переадрисовывает на стрвницу где необходимо ввести остальные данные</returns>
        public IActionResult OnPostFurther(DtoStartItem startItem)
        {
            return RedirectToPage("addsubitem", new { area = "workspace", itemType = startItem.ItemType });
        }

        /// <summary>
        /// AJAX обработчик
        /// </summary>
        /// <returns>Возвращает частичноре представление строки с вводом имен авторов на разных языках</returns>
        public PartialViewResult OnGetAuthorsRow()
            => Partial("_Add_Start_Author_Next", _cryptografy.AuthorsRowId);
    }
}
