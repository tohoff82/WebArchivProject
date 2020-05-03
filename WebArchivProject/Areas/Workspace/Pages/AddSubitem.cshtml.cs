using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// Модель представления формы ввода, зависящий от типа публикации
    /// </summary>
    public class AddSubitemModel : PageModel
    {
        /// <summary>
        /// тип публикации (книга, пост, тезтс)
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// Обработчик страницы по умолчанию
        /// </summary>
        /// <param name="itemType"></param>
        public void OnGet(string itemType)
        {
            ItemType = itemType;
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
    }
}
