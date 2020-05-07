using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WebArchivProject.Contracts;
using WebArchivProject.Extensions;
using WebArchivProject.Models.DTO;

using static WebArchivProject.Helper.StringConstant;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// ������ ��������� �������� ������
    /// </summary>
    public class SearchModel : PageModel
    {
        private readonly IServBooks _servBooks;
        private readonly IServPosts _servPosts;
        private readonly IServTheses _servTheses;
        private readonly IServUserSession _userSession;

        private bool SessionHasExpired
            => _userSession.User.HasExpired();

        public SearchModel(
            IServBooks servBooks,
            IServPosts servPosts,
            IServTheses servTheses,
            IServUserSession userSession)
        {
            _servBooks = servBooks;
            _servPosts = servPosts;
            _servTheses = servTheses;
            _userSession = userSession;
        }

        /// <summary>
        /// ���������� �� ���������
        /// </summary>
        public IActionResult OnGet()
        {
            if (SessionHasExpired) return Redirect("/");

            return Page();
        }

        /// <summary>
        /// AJAX ���������� ��������� ���� ������� ������
        /// </summary>
        /// <returns>���������� ��������� ������������� ���� ������� ������</returns>
        public PartialViewResult OnPostArchive()
            => Partial("_Partial_AllSearch_Result", new DtoSearchResultAll
            {
                BooksPager = _servBooks.GetPaginationResult(1, 3),
                PostsPager = _servPosts.GetPaginationResult(1, 3),
                ThesesPager = _servTheses.GetPaginationResult(1, 3)
            });

        /// <summary>
        /// AJAX ���������� ��������� �� ��������� ����� ������ ������
        /// </summary>
        /// <param name="tableType">��� ������� ��� ���������� �������� ���������</param>
        /// <param name="action">�������� ���������</param>
        /// <returns></returns>
        public PartialViewResult OnPostCurrentArchiveAll(string tableType, string action)
            => (tableType) switch
            {
                BOOK => Partial("_Table_BooksResult", _servBooks.GetPaginationResult(action.ToPageNum(), 3)),
                POST => Partial("_Table_PostsResult", _servPosts.GetPaginationResult(action.ToPageNum(), 3)),
                _ => Partial("_Table_ThesisesResult", _servTheses.GetPaginationResult(action.ToPageNum(), 3))
            };

        public Task OnPostDeleteItem(string tableType, int itemId)
            => (tableType) switch
            {
                BOOK => _servBooks.DeleteFromDbAsync(itemId),
                POST => _servPosts.DeleteFromDbAsync(itemId),
                _ => _servTheses.DeleteFromDbAsync(itemId)
            };

        /// <summary>
        /// �������� �������� ���� ����������
        /// </summary>
        public PartialViewResult OnGetSpinnerWave()
            => Partial("_UI_Spinner_Wave");

        public IActionResult OnGetLogout()
        {
            _userSession.RemoveUserSession();
            return Redirect("/");
        }
    }
}
