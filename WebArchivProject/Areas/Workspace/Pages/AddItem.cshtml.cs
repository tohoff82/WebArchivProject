using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WebArchivProject.Contracts;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// ������ �������� ���������� � ��
    /// </summary>
    public class AddItemModel : PageModel
    {
        /// <summary>
        /// ������ ������������ ���������������� ��������� ��������� �������������
        /// </summary>
        private readonly IServCryptografy _cryptografy;

        /// <summary>
        /// ����������� ������, ����������� ������������� ������
        /// </summary>
        /// <param name="cryptografy"></param>
        public AddItemModel(
            IServCryptografy cryptografy)
        {
            _cryptografy = cryptografy;
        }

        /// <summary>
        /// ���������� �������� �� ���������
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// ���������� ������ "�����"
        /// </summary>
        /// <param name="startItem"></param>
        /// <returns>���������������� �� �������� ��� ���������� ������ ��������� ������</returns>
        public IActionResult OnPostFurther(DtoStartItem startItem)
        {
            return RedirectToPage("addsubitem", new { area = "workspace", itemType = startItem.ItemType });
        }

        /// <summary>
        /// AJAX ����������
        /// </summary>
        /// <returns>���������� ���������� ������������� ������ � ������ ���� ������� �� ������ ������</returns>
        public PartialViewResult OnGetAuthorsRow()
            => Partial("_Add_Start_Author_Next", _cryptografy.AuthorsRowId);
    }
}
