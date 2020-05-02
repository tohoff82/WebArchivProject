using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebArchivProject.Areas.Workspace.Pages
{
    public class IndexModel : PageModel
    {
        public bool HasNotification { get; set; }

        public void OnGet(bool hasNotify)
        {
            HasNotification = hasNotify;
        }
    }
}
