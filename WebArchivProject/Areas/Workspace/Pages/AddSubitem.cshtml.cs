using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// ������ ������������� ����� �����, ��������� �� ���� ����������
    /// </summary>
    public class AddSubitemModel : PageModel
    {
        /// <summary>
        /// ��� ���������� (�����, ����, �����)
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// ���������� �������� �� ���������
        /// </summary>
        /// <param name="itemType"></param>
        public void OnGet(string itemType)
        {
            ItemType = itemType;
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
    }
}
