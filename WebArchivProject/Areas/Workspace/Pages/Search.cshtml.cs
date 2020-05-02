using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// ������ ��������� �������� ������
    /// </summary>
    public class SearchModel : PageModel
    {
        /// <summary>
        /// ���������� �� ���������
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// AJAX ���������� ��������� ���� ������� ������
        /// </summary>
        /// <returns>���������� ��������� ������������� ���� ������� ������</returns>
        public PartialViewResult OnGetArchive()
        {
            return Partial("_Partial_AllSearch_Result");
        }
    }
}
