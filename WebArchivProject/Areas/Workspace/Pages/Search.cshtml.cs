using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebArchivProject.Areas.Workspace.Pages
{
    public class SearchModel : PageModel
    {
        public void OnGet()
        {
        }

        public PartialViewResult OnGetArchive()
        {
            return Partial("_Partial_AllSearch_Result");
        }
    }
}
