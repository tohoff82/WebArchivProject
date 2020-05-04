using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebArchivProject.Contracts;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// ������ �������� ������� �������
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly IServUserSession _userSession;
        private readonly IServAuthorsRows _rowsCash;

        public IndexModel(
            IServUserSession userSession,
            IServAuthorsRows rowsCash)
        {
            _userSession = userSession;
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
        public void OnGet(bool hasNotify)
        {
            HasNotification = hasNotify;
        }

        /// <summary>
        /// ���������� ������� ������ ��������
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostAdd()
        {
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
