using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// ������ �������� ������� �������
    /// </summary>
    public class IndexModel : PageModel
    {
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
    }
}
