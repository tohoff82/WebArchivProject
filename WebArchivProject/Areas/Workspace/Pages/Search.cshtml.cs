using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// ћодель стартовой страницы поиска
    /// </summary>
    public class SearchModel : PageModel
    {
        /// <summary>
        /// обработчик по умолчанию
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// AJAX обработчик получени€ всех записей архива
        /// </summary>
        /// <returns>¬озвращает частичное представление всех записей архива</returns>
        public PartialViewResult OnGetArchive()
        {
            return Partial("_Partial_AllSearch_Result");
        }
    }
}
