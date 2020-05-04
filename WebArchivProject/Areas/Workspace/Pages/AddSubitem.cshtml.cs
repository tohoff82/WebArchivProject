using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// ������ ������������� ����� �����, ��������� �� ���� ����������
    /// </summary>
    public class AddSubitemModel : PageModel
    {
        private readonly IServUserSession _userSession;

        private bool SessionHasExpired
            => _userSession.User.HasExpired();

        /// <summary>
        /// ��� ���������� (�����, ����, �����)
        /// </summary>
        public string ItemType { get; set; }

        public AddSubitemModel(
            IServUserSession userSession)
        {
            _userSession = userSession;
        }

        /// <summary>
        /// ���������� �������� �� ���������
        /// </summary>
        public IActionResult OnGet(string itemType)
        {
            if (SessionHasExpired) return Redirect("/");

            ItemType = itemType;
            return Page();
        }

        /// <summary>
        /// ���������� ������ "�����"
        /// </summary>
        /// <returns>���������������� �� ��� �����</returns>
        public IActionResult OnPostBack()
        {
            return RedirectToPage("AddItem", new { area = "Workspace" });
        }

        /// <summary>
        /// ���������� ������ "��������"
        /// </summary>
        /// <returns>��������� ������ ���������� �������� � �� � ���������������� �� ������� �������� ������� �������</returns>
        public IActionResult OnPostAdd()
        {
            TempData["Notification"] = "��� ����� ������ ������ �� ������!";

            return RedirectToPage("/Index", new { area = "Workspace", hasNotify = true });
        }

        public IActionResult OnGetLogout()
        {
            _userSession.RemoveUserSession();
            return Redirect("/");
        }
    }
}
