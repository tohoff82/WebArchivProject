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

        public IndexModel(
            IServUserSession userSession)
        {
            _userSession = userSession;
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
