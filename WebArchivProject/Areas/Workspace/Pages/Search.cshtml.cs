using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;
using WebArchivProject.Models;
using WebArchivProject.Models.DTO;
using WebArchivProject.Models.SearchFilters;
using static WebArchivProject.Helper.StringConstant;

namespace WebArchivProject.Areas.Workspace.Pages
{
    /// <summary>
    /// Модель страницы с поисковыми фильтрами
    /// </summary>
    public class SearchModel : PageModel
    {
        private readonly IServBooks _servBooks;
        private readonly IServPosts _servPosts;
        private readonly IServTheses _servTheses;
        private readonly IServUserSession _userSession;
        private readonly PagerSettings _pagerSettings;

        private bool SessionHasExpired
            => _userSession.User.HasExpired();

        public SearchModel(
            IServBooks servBooks,
            IServPosts servPosts,
            IServTheses servTheses,
            IServUserSession userSession,
            IOptions<MySettings> options)
        {
            _servBooks = servBooks;
            _servPosts = servPosts;
            _servTheses = servTheses;
            _userSession = userSession;
            _pagerSettings = options.Value.PagerSettings;
        }

        /// <summary>
        /// обработчик по умолчанию
        /// </summary>
        public IActionResult OnGet()
        {
            if (SessionHasExpired) return Redirect("/");

            return Page();
        }

        /// <summary>
        /// AJAX обработчик фильтра книг
        /// </summary>
        /// <param name="year">год</param>
        /// <param name="author">автор</param>
        /// <param name="name">название </param>
        /// <returns>частичное представление таблицы с пейджингом</returns>
        public PartialViewResult OnPostBooksSearchFilter(string year, string author, string name)
            => Partial("_Table_BooksResult", _servBooks.GetPaginatorResultModal(new BooksSearchFilter
            {
                BookYear = year,
                BookName = name,
                AuthorName = author,
                Target = MODAL_CONTAINER
            }));

        public PartialViewResult OnPostModalSearchPagination(string tableType, string action, string target)
            => (tableType) switch
            {
                BOOK => Partial("_Table_BooksResult", _servBooks.GetBooksSearchPaginator(action.ToPageNum(), _pagerSettings.ItemPerPage, target))
            };

        /// <summary>
        /// AJAX обработчик получения всех записей архива
        /// </summary>
        /// <returns>Возвращает частичное представление всех записей архива</returns>
        public PartialViewResult OnPostArchive()
            => Partial("_Partial_AllSearch_Result", new DtoSearchResultAll
            {
                BooksPager = _servBooks.GetPaginationResult(1, _pagerSettings.ItemPerPage, ALL_CONTAINER),
                PostsPager = _servPosts.GetPaginationResult(1, _pagerSettings.ItemPerPage, ALL_CONTAINER),
                ThesesPager = _servTheses.GetPaginationResult(1, _pagerSettings.ItemPerPage, ALL_CONTAINER)
            });

        /// <summary>
        /// AJAX обработчик пагинации на карточках всего списка архива
        /// </summary>
        /// <param name="tableType">тип таблице где происходит действие пагинации</param>
        /// <param name="action">параметр пагинации</param>
        /// <returns></returns>
        public PartialViewResult OnPostCurrentArchiveAll(string tableType, string action, string target)
            => (tableType) switch
            {
                BOOK => Partial("_Table_BooksResult", _servBooks.GetPaginationResult(action.ToPageNum(), _pagerSettings.ItemPerPage, target)),
                POST => Partial("_Table_PostsResult", _servPosts.GetPaginationResult(action.ToPageNum(), _pagerSettings.ItemPerPage, target)),
                _ => Partial("_Table_ThesisesResult", _servTheses.GetPaginationResult(action.ToPageNum(), _pagerSettings.ItemPerPage, target))
            };

        /// <summary>
        /// AJAX обработчик удаления элемента в таблицах фильтра "Все публикации"
        /// </summary>
        /// <param name="tableType">тип таблицы в каторой происходит удаление</param>
        /// <param name="itemId">идентификатор удаляемого элемента</param>
        public Task OnPostDeleteItem(string tableType, int itemId)
            => (tableType) switch
            {
                BOOK => _servBooks.DeleteFromDbAsync(itemId),
                POST => _servPosts.DeleteFromDbAsync(itemId),
                _ => _servTheses.DeleteFromDbAsync(itemId)
            };

        /// <summary>
        /// Эмуляция загрузки
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
