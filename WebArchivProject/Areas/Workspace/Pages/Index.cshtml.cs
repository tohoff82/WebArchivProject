using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// ������ �������� ������� �������
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly IServUserSession _userSession;
        private readonly IServStartItems _startItems;
        private readonly IServAuthorsRows _rowsCash;

        private bool SessionHasExpired
            => _userSession.User.HasExpired();

        public IndexModel(
            IServUserSession userSession,
            IServStartItems startItems,
            IServAuthorsRows rowsCash)
        {
            _userSession = userSession;
            _startItems = startItems;
            _rowsCash = rowsCash;
        }

        /// <summary>
        /// ���� ���������� �� ����� ����������� �� �������� ���������� �������� � ��
        /// </summary>
        public bool HasNotification { get; set; }

        /// <summary>
        /// ���������� ��������
        /// </summary>
        /// <param name="hasNotify"></param>
        public IActionResult OnGet(bool hasNotify)
        {
            if (SessionHasExpired) return Redirect("/");

            HasNotification = hasNotify;
            return Page();
        }

        /// <summary>
        /// ���������� ������� ������ ��������
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostAdd()
        {
            if (SessionHasExpired) return Redirect("/");

            _startItems.InitStartItemCash();
            _rowsCash.InitAuthorsRowsCash();
            return RedirectToPage("AddItem", new { area = "Workspace" });
        }

        /// <summary>
        /// ���������� ������ � �����
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGetLogout()
        {
            _userSession.RemoveUserSession();
            return Redirect("/");
        }
    }
}
