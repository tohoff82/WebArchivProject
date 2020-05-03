using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebArchivProject.Contracts;
using WebArchivProject.Models.DTO;
using static WebArchivProject.Helper.StringConstant;

namespace WebArchivProject.Areas.Identity.Pages
{
    /// <summary>
    /// ������ �������� identity
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly IAuthManager _authManager;

        [BindProperty(SupportsGet = true)]
        public string IdentityState { get; set; }

        public IndexModel(
            IAuthManager authManager)
        {
            _authManager = authManager;
        }

        /// <summary>
        /// ���� ������ � �������� ��� ����������� - ���������������� �� ���������
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
            => Redirect("/");

        /// <summary>
        /// ���������� ��������� �������� � �������
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGetLogin()
        {
            IdentityState = LOGIN;
            return Page();
        }

        /// <summary>
        /// ���������� ��������� �������� � ������������
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGetRegister()
        {
            IdentityState = REGISTER;
            return Page();
        }

        /// <summary>
        /// ���������� ����������� ������������
        /// </summary>
        /// <returns>� ������ ������ ���������������� � ������� �������</returns>
        public async Task<IActionResult> OnPostLogin(DtoFormLoginUser loginUser)
        {
            var answ = await _authManager.LoginAsync(loginUser);

            if (!answ.IsSuccess)
            {
                TempData["LoginNotification"] = answ.Reason;
                IdentityState = LOGIN;
                return Page();
            }
            else
            {
                TempData["Notification"] = answ.Reason;
                return RedirectToPage("/Index", new
                { 
                    area = "Workspace", 
                    hasNotify = true 
                });
            }
        }

        /// <summary>
        /// ���������� ����������� ������ ������������
        /// </summary>
        /// <returns>���������������� �� ������� ������� �����</returns>
        public async Task<IActionResult> OnPostRegister(DtoFormRegisterUser registerUser)
        {
            registerUser.Role = ROLE_USER;
            await _authManager.RegisterAsync(registerUser);

            TempData["Notification"] = "��� �������� ����� ������ ��������!";

            return RedirectToPage("/Index", new { area = "Workspace", hasNotify = true });
        }
    }
}
