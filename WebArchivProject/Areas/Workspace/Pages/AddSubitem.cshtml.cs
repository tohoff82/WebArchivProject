using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// Модель представления формы ввода, зависящий от типа публикации
    /// </summary>
    public class AddSubitemModel : PageModel
    {
        private readonly IServUserSession _userSession;

        private bool SessionHasExpired
            => _userSession.User.HasExpired();

        /// <summary>
        /// тип публикации (книга, пост, тезтс)
        /// </summary>
        public string ItemType { get; set; }

        public AddSubitemModel(
            IServUserSession userSession)
        {
            _userSession = userSession;
        }

        /// <summary>
        /// Обработчик страницы по умолчанию
        /// </summary>
        public IActionResult OnGet(string itemType)
        {
            if (SessionHasExpired) return Redirect("/");

            ItemType = itemType;
            return Page();
        }

        /// <summary>
        /// Обработчик кнопки "Назад"
        /// </summary>
        /// <returns>Переадресовывает на шаг назад</returns>
        public IActionResult OnPostBack()
        {
            return RedirectToPage("AddItem", new { area = "Workspace" });
        }

        /// <summary>
        /// Обработчик кнопки "Добавить"
        /// </summary>
        /// <returns>Реализует логику добавления элемента в БД и переадресовывает на главную страницу рабочей области</returns>
        public IActionResult OnPostAdd()
        {
            TempData["Notification"] = "Ваш запис успішно додано до архіву!";

            return RedirectToPage("/Index", new { area = "Workspace", hasNotify = true });
        }

        public IActionResult OnGetLogout()
        {
            _userSession.RemoveUserSession();
            return Redirect("/");
        }
    }
}
