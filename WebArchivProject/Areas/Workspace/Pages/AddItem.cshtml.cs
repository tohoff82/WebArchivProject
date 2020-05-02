using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WebArchivProject.Contracts;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Areas.Workspace.Pages
{
    public class AddItemModel : PageModel
    {
        private readonly IServCryptografy _cryptografy;

        public AddItemModel(
            IServCryptografy cryptografy)
        {
            _cryptografy = cryptografy;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostFurther(DtoStartItem startItem)
        {
            return RedirectToPage("addsubitem", new { area = "workspace", itemType = startItem.ItemType });
        }

        public PartialViewResult OnGetAuthorsRow()
            => Partial("_Add_Start_Author_Next", _cryptografy.AuthorsRowId);
    }
}
