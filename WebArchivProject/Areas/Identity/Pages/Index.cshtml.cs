using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using static WebArchivProject.Helper.StringConstant;

namespace WebArchivProject.Areas.Identity.Pages
{
    /// <summary>
    /// ������ �������� identity
    /// </summary>
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string IdentityState { get; set; }

        /// <summary>
        /// ���� ������ � �������� ��� ����������� - ���������������� �� ���������
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
            => Redirect("/");

        /// <summary>
        /// ���������� �������� � �������
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGetLogin()
        {
            IdentityState = LOGIN;
            return Page();
        }

        /// <summary>
        /// ���������� � ������� � ������������
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGetRegister()
        {
            IdentityState = REGISTER;
            return Page();
        }
    }
}
