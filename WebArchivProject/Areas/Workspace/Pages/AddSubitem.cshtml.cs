using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Areas.Workspace.Pages
{
    public class AddSubitemModel : PageModel
    {
        public string ItemType { get; set; }

        public void OnGet(string itemType)
        {
            ItemType = itemType;
        }

        public IActionResult OnPostBack()
        {
            return RedirectToPage("AddItem", new { area = "Workspace" });
        }

        public IActionResult OnPostAdd()
        {
            return RedirectToPage("/Index", new { area = "Workspace", hasNotify = true });
        }
    }
}
