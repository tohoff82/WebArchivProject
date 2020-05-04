using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebArchivProject.Contracts;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// модель страницы рабочей области
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly IServUserSession _userSession;
        private readonly IServAuthorsRows _rowsCash;

        public IndexModel(
            IServUserSession userSession,
            IServAuthorsRows rowsCash)
        {
            _userSession = userSession;
            _rowsCash = rowsCash;
        }

        /// <summary>
        /// поле отвечающее за вывод уведомления об успешном добавление элемента в БД
        /// </summary>
        public bool HasNotification { get; set; }

        /// <summary>
        /// обработчик страницы
        /// </summary>
        /// <param name="hasNotify"></param>
        public void OnGet(bool hasNotify)
        {
            HasNotification = hasNotify;
        }

        /// <summary>
        /// Обработчик нажатия кнопки Добавить
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostAdd()
        {
            _rowsCash.InitAuthorsRowsCash();
            return RedirectToPage("AddItem", new { area = "Workspace" });
        }

        /// <summary>
        /// Обработчик выхода с сайта
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGetLogout()
        {
            _userSession.RemoveUserSession();
            return Redirect("/");
        }
    }
}
