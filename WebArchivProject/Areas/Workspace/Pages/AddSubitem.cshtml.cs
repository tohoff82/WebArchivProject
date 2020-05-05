using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;
using WebArchivProject.Models.DTO;
using static WebArchivProject.Helper.StringConstant;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// ������ ������������� ����� �����, ��������� �� ���� ����������
    /// </summary>
    public class AddSubitemModel : PageModel
    {
        private readonly IServStartItems _servStartItems;
        private readonly IServUserSession _userSession;
        private readonly IMapper _mapper;

        private bool SessionHasExpired
            => _userSession.User.HasExpired();

        /// <summary>
        /// ��� ���������� (�����, ����, �����)
        /// </summary>
        public string ItemType { get; set; }

        public AddSubitemModel(
            IServStartItems servStartItems,
            IServUserSession userSession,
            IMapper mapper)
        {
            _servStartItems = servStartItems;
            _userSession = userSession;
            _mapper = mapper;
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
            if (SessionHasExpired) return Redirect("/");

            return RedirectToPage("AddItem", new { area = "Workspace" });
        }

        /// <summary>
        /// ���������� ������ "��������"
        /// </summary>
        /// <returns>��������� ������ ���������� �������� � �� � ���������������� �� ������� �������� ������� �������</returns>
        public IActionResult OnPostAddBook(DtoFormBook formBook)
        {
            if (SessionHasExpired) return Redirect("/");

            var dtoBook = _mapper.Map<DtoBook>(_servStartItems.StartItem);
            
            var dto = _mapper.Map(formBook, dtoBook);
            // to do: send dto to reposervice


            TempData["Notification"] = "��� ����� ������ ������ �� ������!";

            return RedirectToPage("/Index", new { area = "Workspace", hasNotify = true });
        }
        public IActionResult OnPostAddPost(DtoFormPost formPost)
        {
            if (SessionHasExpired) return Redirect("/");

            var dtoPost = _mapper.Map<DtoPost>(_servStartItems.StartItem);

            var dto = _mapper.Map(formPost, dtoPost);
            // to do: send dto to reposervice

            TempData["Notification"] = "��� ����� ������ ������ �� ������!";

            return RedirectToPage("/Index", new { area = "Workspace", hasNotify = true });
        }
        public IActionResult OnPostAddThesis(DtoFormThesis formThesis)
        {
            if (SessionHasExpired) return Redirect("/");

            var dtoThesis = _mapper.Map<DtoThesis>(_servStartItems.StartItem);

            var dto = _mapper.Map(formThesis, dtoThesis);
            // to do: send dto to reposervice

            TempData["Notification"] = "��� ����� ������ ������ �� ������!";

            return RedirectToPage("/Index", new { area = "Workspace", hasNotify = true });
        }

        public PartialViewResult OnPostTypeSwitcher(string type)
        {
            if (type == BOOK) return Partial("_Select_For_BookItem");
            else return Partial("_Select_For_MethodItem");
        }

        public IActionResult OnGetLogout()
        {
            _userSession.RemoveUserSession();
            return Redirect("/");
        }
    }
}
