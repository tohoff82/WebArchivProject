using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Linq;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// Модель страницы добавления в БД
    /// </summary>
    public class AddItemModel : PageModel
    {
        [BindProperty]
        public DtoStartItem DtoStartItem { get; set; }

        private readonly IServUserSession _userSession;
        private readonly IServStartItems _startItems;
        private readonly IServAuthorsRows _rowsCash;

        private bool SessionHasExpired
            => _userSession.User.HasExpired();

        /// <summary>
        /// конструктор модели, принимающий вышеуказанный сервис
        /// </summary>
        public AddItemModel(
            IServUserSession userSession,
            IServStartItems startItems,
            IServAuthorsRows rowsCash)
        {
            _userSession = userSession;
            _startItems = startItems;
            _rowsCash = rowsCash;
        }

        /// <summary>
        /// обработчик страницы по умолчанию
        /// </summary>
        public IActionResult OnGet()
        {
            if (SessionHasExpired) return Redirect("/");

            return Page();
        }

        /// <summary>
        /// обработчик кнопки "Далее"
        /// </summary>
        /// <param name="startItem"></param>
        /// <returns>Переадрисовывает на стрвницу где необходимо ввести остальные данные</returns>
        public IActionResult OnPostFurther(DtoStartItem startItem)
        {
            _startItems.UpdateStartItem(startItem);
            _rowsCash.HandleUpdateRow(startItem.Authors.Skip(1).ToList());
            return RedirectToPage("addsubitem", new { area = "workspace", itemType = DtoStartItem.ItemType });
        }

        /// <summary>
        /// AJAX обработчик кнопки +
        /// </summary>
        /// <returns>Возвращает частичноре представление области ввода имен авторов на разных языках</returns>
        public PartialViewResult OnPostAddRow(string[] names)
        {
            if (names != null && names.Length > 0) _rowsCash.HandleAddRow(names.ToDtoAuthors());
            return Partial("_Add_Start_Author_Next", DtoStartItem);
        }

        /// <summary>
        /// AJAX обработчик кнопки -
        /// </summary>
        /// <returns>Возвращает частичноре представление области ввода имен авторов на разных языках</returns>
        public PartialViewResult OnPostDeleteRow(string[] names)
        {
            _rowsCash.HandleUpdateRow(names.ToDtoAuthors());
            if (names.Length == 0) return Partial("_Empty_Block");
            else return Partial("_Add_Start_Author_Next", DtoStartItem);
        }

        public IActionResult OnGetLogout()
        {
            _userSession.RemoveUserSession();
            return Redirect("/");
        }
    }
}
