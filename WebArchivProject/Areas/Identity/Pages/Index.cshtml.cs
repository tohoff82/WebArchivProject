using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using static WebArchivProject.Helper.StringConstant;

namespace WebArchivProject.Areas.Identity.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string IdentityState { get; set; }

        public IActionResult OnGet()
            => Redirect("/");

        public IActionResult OnGetLogin()
        {
            IdentityState = LOGIN;
            return Page();
        }

        public IActionResult OnGetRegister()
        {
            IdentityState = REGISTER;
            return Page();
        }
    }
}
