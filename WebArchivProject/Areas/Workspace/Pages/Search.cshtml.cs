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
        private readonly IServExport _servExport;
        private readonly IServEditItem _servEditItem;
        private readonly IServUserSession _userSession;
        private readonly PagerSettings _pagerSettings;

        private bool SessionHasExpired
            => _userSession.User.HasExpired();

        public SearchModel(
            IServBooks servBooks,
            IServPosts servPosts,
            IServTheses servTheses,
            IServExport servExport,
            IServEditItem servEditItem,
            IServUserSession userSession,
            IOptions<MySettings> options)
        {
            _servBooks = servBooks;
            _servPosts = servPosts;
            _servTheses = servTheses;
            _servExport = servExport;
            _servEditItem = servEditItem;
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

        /// <summary>
        /// AJAX обработчик фильтра статей
        /// </summary>
        /// <param name="year">год</param>
        /// <param name="author">автор</param>
        /// <param name="name">название </param>
        /// <param name="magazine">название журнала</param>
        /// <returns></returns>
        public PartialViewResult OnPostPostsSearchFilter(string year, string author, string name, string magazine)
            => Partial("_Table_PostsResult", _servPosts.GetPaginatorResultModal(new PostsSearchFilter
            {
                PostYear = year,
                PostName = name,
                Magazine = magazine,
                AuthorName = author,
                Target = MODAL_CONTAINER
            }));

        /// <summary>
        /// AJAX обработчик фильтра тезисов
        /// </summary>
        /// <param name="year">год</param>
        /// <param name="author">автор</param>
        /// <param name="name">название </param>
        /// <param name="pages">страницы</param>
        /// <returns></returns>
        public PartialViewResult OnPostThesesSearchFilter(string year, string author, string name, string pages)
            => Partial("_Table_ThesisesResult", _servTheses.GetPaginatorResultModal(new ThesesSearchFilter
            {
                Pages = pages,
                ThesisYear = year,
                ThesisName = name,
                AuthorName = author,
                Target = MODAL_CONTAINER
            }));

        /// <summary>
        /// AJAX обработчик паганации в модальных окнах
        /// </summary>
        /// <param name="tableType">тип таблицы в модальном окне</param>
        /// <param name="action">данные пагинатора (вперед, назад, 1, 2, 3...)</param>
        /// <param name="target">целевой контейнер</param>
        /// <returns></returns>
        public PartialViewResult OnPostModalSearchPagination(string tableType, string action, string target)
            => (tableType) switch
            {
                BOOK => Partial("_Table_BooksResult", _servBooks.GetBooksSearchPaginator
                    (
                        pageNumber: action.ToPageNum(),
                        pageSize: _pagerSettings.ItemPerPage,
                        target: target)
                    ),
                POST => Partial("_Table_PostsResult", _servPosts.GetPostsSearchPaginator
                    (
                        pageNumber: action.ToPageNum(),
                        pageSize: _pagerSettings.ItemPerPage,
                        target: target)
                    ),
                _ => Partial("_Table_ThesisesResult", _servTheses.GetThesesSearchPaginator
                    (
                        pageNumber: action.ToPageNum(),
                        pageSize: _pagerSettings.ItemPerPage,
                        target: target)
                    )
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
        /// AJAX щбработчик видимости кнопки Экспорта
        /// </summary>
        /// <param name="target"></param>
        public JsonResult OnPostExportDisabled(string target)
            => (target) switch
            {
                BOOK => new JsonResult(_servExport.BooksExportDisabled),
                POST => new JsonResult(_servExport.PostsExportDisabled),
                _ => new JsonResult(_servExport.ThesesExportDisabled)
            };

        /// <summary>
        /// Обработчик Экспорта
        /// </summary>
        /// <param name="target"></param>
        public IActionResult OnPostExport(string target)
            => (target) switch
            {
                BOOK => File(_servExport.GetExelBooksBuffer(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Books.xlsx"),
                POST => File(_servExport.GetExelPostsBuffer(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Posts.xlsx"),
                _ => File(_servExport.GetExelThesesBuffer(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Theses.xlsx")
            };

        public async Task<PartialViewResult> OnPostEditedItemAsync(string tableType)
            => (tableType.ToTarget()) switch
            {
                BOOK => Partial("_Row_BookResult_Edit", await _servBooks.GetFromDbAsync(tableType.ToItemId()))
            };

        public async Task<IActionResult> OnPostEditBook(DtoBookEdit bookEdit)
        {
            await _servEditItem.EditBookAsync(bookEdit);
            return Page();
        }

        /// <summary>
        /// Эмуляция загрузки
        /// </summary>
        public PartialViewResult OnGetSpinnerWave()
            => Partial("_UI_Spinner_Wave");


        /// <summary>
        /// Обработчик выхода с сайта
        /// </summary>
        public IActionResult OnGetLogout()
        {
            _userSession.RemoveUserSession();
            return Redirect("/");
        }
    }
}
