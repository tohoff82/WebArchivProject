using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// ������ ��������� �������� ������
    /// </summary>
    public class SearchModel : PageModel
    {
        private readonly IServUserSession _userSession;

        private bool SessionHasExpired
            => _userSession.User.HasExpired();

        public SearchModel(
            IServUserSession userSession)
        {
            _userSession = userSession;
        }

        /// <summary>
        /// ���������� �� ���������
        /// </summary>
        public IActionResult OnGet()
        {
            if (SessionHasExpired) return Redirect("/");

            return Page();
        }

        /// <summary>
        /// AJAX ���������� ��������� ���� ������� ������
        /// </summary>
        /// <returns>���������� ��������� ������������� ���� ������� ������</returns>
        public PartialViewResult OnGetArchive()
        {
            return Partial("_Partial_AllSearch_Result");
        }

        public IActionResult OnGetLogout()
        {
            _userSession.RemoveUserSession();
            return Redirect("/");
        }
    }
}
