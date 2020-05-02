using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// модель страницы рабочей области
    /// </summary>
    public class IndexModel : PageModel
    {
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
    }
}
