using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Threading.Tasks;

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
        private readonly IServUserSession _userSession;

        [BindProperty(SupportsGet = true)]
        public string IdentityState { get; set; }

        public IndexModel(
            IAuthManager authManager,
            IServUserSession userSession)
        {
            _authManager = authManager;
            _userSession = userSession;
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
                TempData["Notification"] = string.Format
                (
                    format: answ.Reason,
                    _userSession.User.Name
                );
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
            var answ = await _authManager.RegisterAsync(registerUser);

            if (!answ.IsSuccess)
            {
                TempData["RegNotify"] = answ.Reason;
                TempData["RegClass"] = "regNotify";
                IdentityState = REGISTER;
                return Page();
            }

            TempData["Notification"] = string.Format(answ.Reason, registerUser.Name);

            return RedirectToPage("/Index", new { area = "Workspace", hasNotify = true });
        }
    }
}
