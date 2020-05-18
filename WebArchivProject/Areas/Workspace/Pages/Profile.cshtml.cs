using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WebArchivProject.Contracts;
using WebArchivProject.Extensions;
using WebArchivProject.Models;
using WebArchivProject.Models.DTO;
using static WebArchivProject.Helper.StringConstant;

namespace WebArchivProject.Areas.Workspace.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IAuthManager _authManager;
        private readonly IServUserSession _userSession;

        private bool SessionHasExpired
            => _userSession.User.HasExpired();

        public SessionUser EditedUser
            => _userSession.User;

        public string DisabledRole
            => _userSession.User.Role == ROLE_ADMIN
                ? "disabled" : null;

        public ProfileModel(
            IAuthManager authManager,
            IServUserSession userSession)
        {
            _authManager = authManager;
            _userSession = userSession;
        }

        /// <summary>
        /// Получение страницы редактирования
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            if (SessionHasExpired) return Redirect("/");

            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(DtoFormEditUser editUser)
        {
            var answ = await _authManager.EditUserAsync(editUser);

            TempData["Notification"] = "Ваші дані успішно змшнено!";

            return RedirectToPage("/Index", new { area = "Workspace", hasNotify = true });
        }

        /// <summary>
        /// Обработчик выхода с сайта
        /// </summary>
        public IActionResult OnGetLogout()
        {
            _userSession.RemoveUserSession();
            return Redirect("/");
        }
    }
}
