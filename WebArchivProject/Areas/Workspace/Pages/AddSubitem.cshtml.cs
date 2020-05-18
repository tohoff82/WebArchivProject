using AutoMapper;
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
    /// Модель представления формы ввода, зависящий от типа публикации
    /// </summary>
    public class AddSubitemModel : PageModel
    {
        private readonly IServStartItemsCash _servStartItems;
        private readonly IServUserSession _userSession;
        private readonly IServTheses _servTheses;
        private readonly IServBooks _servBooks;
        private readonly IServPosts _servPosts;
        private readonly IMapper _mapper;

        private bool SessionHasExpired
            => _userSession.User.HasExpired();

        /// <summary>
        /// тип публикации (книга, пост, тезтс)
        /// </summary>
        public string ItemType { get; set; }

        public AddSubitemModel(
            IServStartItemsCash servStartItems,
            IServUserSession userSession,
            IServTheses servTheses,
            IServBooks servBooks,
            IServPosts servPosts,
            IMapper mapper)
        {
            _servStartItems = servStartItems;
            _userSession = userSession;
            _servTheses = servTheses;
            _servBooks = servBooks;
            _servPosts = servPosts;
            _mapper = mapper;
        }

        /// <summary>
        /// Обработчик страницы по умолчанию
        /// </summary>
        public IActionResult OnGet(string itemType)
        {
            if (SessionHasExpired) return Redirect("/");

            ItemType = itemType;
            return Page();
        }

        /// <summary>
        /// Обработчик кнопки "Назад"
        /// </summary>
        /// <returns>Переадресовывает на шаг назад</returns>
        public IActionResult OnPostBack()
        {
            if (SessionHasExpired) return Redirect("/");

            return RedirectToPage("AddItem", new { area = "Workspace" });
        }

        /// <summary>
        /// Обработчик кнопки "Добавить" книгу
        /// </summary>
        /// <returns>Реализует логику добавления элемента в БД и переадресовывает на главную страницу рабочей области</returns>
        public async Task<IActionResult> OnPostAddBook(DtoFormBook formBook)
        {
            if (SessionHasExpired) return Redirect("/");

            await _servBooks.AddToDbAsync(_mapper.Map(formBook,
                _mapper.Map<DtoBook>(_servStartItems.StartItem)));

            TempData["Notification"] = "Ваш запис успішно додано до архіву!";

            return RedirectToPage("/Index", new { area = "Workspace", hasNotify = true });
        }
        /// <summary>
        /// Обработчик кнопки "Добавить" Публикацию
        /// </summary>
        /// <returns>Реализует логику добавления элемента в БД и переадресовывает на главную страницу рабочей области</returns>
        public async Task<IActionResult> OnPostAddPost(DtoFormPost formPost)
        {
            if (SessionHasExpired) return Redirect("/");

            await _servPosts.AddToDbAsync(_mapper.Map(formPost,
                _mapper.Map<DtoPost>(_servStartItems.StartItem)));

            TempData["Notification"] = "Ваш запис успішно додано до архіву!";

            return RedirectToPage("/Index", new { area = "Workspace", hasNotify = true });
        }
        /// <summary>
        /// Обработчик кнопки "Добавить" тезис
        /// </summary>
        /// <returns>Реализует логику добавления элемента в БД и переадресовывает на главную страницу рабочей области</returns>
        public async Task<IActionResult> OnPostAddThesis(DtoFormThesis formThesis)
        {
            if (SessionHasExpired) return Redirect("/");

            await _servTheses.AddToDbAsync(_mapper.Map(formThesis,
                _mapper.Map<DtoThesis>(_servStartItems.StartItem)));

            TempData["Notification"] = "Ваш запис успішно додано до архіву!";

            return RedirectToPage("/Index", new { area = "Workspace", hasNotify = true });
        }

        /// <summary>
        /// переключатель типа книга - методичка
        /// </summary>
        public PartialViewResult OnPostTypeSwitcher(string type, string target)
        {
            if (type == BOOK) return Partial("_Select_For_BookItem", target ?? null);
            else return Partial("_Select_For_MethodItem", target ?? null);
        }

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
