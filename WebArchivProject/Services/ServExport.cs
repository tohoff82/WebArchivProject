using ClosedXML.Excel;

using System.IO;

using WebArchivProject.Contracts;

using static WebArchivProject.Extensions.StringExtensions;

namespace WebArchivProject.Services
{
    class ServExport : IServExport
    {
        private readonly IServBooks _servBooks;
        private readonly IServPosts _servPosts;
        private readonly IServTheses _servTheses;

        public bool BooksExportDisabled
            => GetBooksDisabled();

        public bool PostsExportDisabled
            => GetPostsDisabled();

        public bool ThesesExportDisabled
            => GetThesesDisabled();

        public ServExport(
            IServBooks servBooks,
            IServPosts servPosts,
            IServTheses servTheses)
        {
            _servBooks = servBooks;
            _servPosts = servPosts;
            _servTheses = servTheses;
        }

        /// <summary>
        /// Формируем буфер импорта книг
        /// </summary>
        public byte[] GetExelBooksBuffer()
        {
            int currentRow = 1;
            int currentCell = 0;
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Книжки/Методички");
            AddBookTitles(currentRow, worksheet);
            foreach (var book in _servBooks.GetSearchCash())
            {
                currentRow++;
                worksheet.Cell(currentRow, ++currentCell).Value = book.Id;
                worksheet.Cell(currentRow, ++currentCell).Value = book.AuthorFirst;
                worksheet.Cell(currentRow, ++currentCell).Value = book.AuthorsNext.ToXLCell();
                worksheet.Cell(currentRow, ++currentCell).Value = book.Name;
                worksheet.Cell(currentRow, ++currentCell).Value = book.Year;
                worksheet.Cell(currentRow, ++currentCell).Value = book.MaxPageCount;
                worksheet.Cell(currentRow, ++currentCell).Value = book.Type;
                worksheet.Cell(currentRow, ++currentCell).Value = book.IssuerLine;
                currentCell = 0;
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        /// <summary>
        /// Формируем буфер импорта публикаций
        /// </summary>
        public byte[] GetExelPostsBuffer()
        {
            int currentRow = 1;
            int currentCell = 0;
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Публікації");
            AddPostsTitles(currentRow, worksheet);
            foreach (var post in _servPosts.GetSearchCash())
            {
                currentRow++;
                worksheet.Cell(currentRow, ++currentCell).Value = post.Id;
                worksheet.Cell(currentRow, ++currentCell).Value = post.Authors.ToXLCell();
                worksheet.Cell(currentRow, ++currentCell).Value = post.Name;
                worksheet.Cell(currentRow, ++currentCell).Value = post.Magazine;
                worksheet.Cell(currentRow, ++currentCell).Value = post.Year;
                worksheet.Cell(currentRow, ++currentCell).Value = post.TomName;
                worksheet.Cell(currentRow, ++currentCell).Value = post.PagesCount;
                worksheet.Cell(currentRow, ++currentCell).Value = post.PagesInterval;
                currentCell = 0;
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        /// <summary>
        /// Формируем буфер импорта тезисов
        /// </summary>
        public byte[] GetExelThesesBuffer()
        {
            int currentRow = 1;
            int currentCell = 0;
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Тези");
            AddThesesTitles(currentRow, worksheet);
            foreach (var thesis in _servTheses.GetSearchCash())
            {
                currentRow++;
                worksheet.Cell(currentRow, ++currentCell).Value = thesis.Id;
                worksheet.Cell(currentRow, ++currentCell).Value = thesis.Authors.ToXLCell();
                worksheet.Cell(currentRow, ++currentCell).Value = thesis.Name;
                worksheet.Cell(currentRow, ++currentCell).Value = thesis.ConferenceName;
                worksheet.Cell(currentRow, ++currentCell).Value = thesis.Location;
                worksheet.Cell(currentRow, ++currentCell).Value = thesis.Year;
                worksheet.Cell(currentRow, ++currentCell).Value = thesis.PagesCount;
                worksheet.Cell(currentRow, ++currentCell).Value = thesis.PagesInterval;
                currentCell = 0;
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        /// <summary>
        /// Формируем в Ексель файле заголовки для книг
        /// </summary>
        /// <param name="row"></param>
        /// <param name="worksheet"></param>
        private void AddBookTitles(int row, IXLWorksheet worksheet)
        {
            worksheet.Cell(row, 1).Value = "№";
            worksheet.Cell(row, 2).Value = "Перший автор";
            worksheet.Cell(row, 3).Value = "Інші автори";
            worksheet.Cell(row, 4).Value = "Назва";
            worksheet.Cell(row, 5).Value = "Рік";
            worksheet.Cell(row, 6).Value = "Сторінок";
            worksheet.Cell(row, 7).Value = "Тип";
            worksheet.Cell(row, 8).Value = "Видавництво";
        }

        /// <summary>
        /// Формируем в Ексель файле заголовки для публикаций
        /// </summary>
        /// <param name="row"></param>
        /// <param name="worksheet"></param>
        private void AddPostsTitles(int row, IXLWorksheet worksheet)
        {
            worksheet.Cell(row, 1).Value = "№";
            worksheet.Cell(row, 2).Value = "Автор";
            worksheet.Cell(row, 3).Value = "Назва статті";
            worksheet.Cell(row, 4).Value = "Журнал";
            worksheet.Cell(row, 5).Value = "Рік";
            worksheet.Cell(row, 6).Value = "Том";
            worksheet.Cell(row, 7).Value = "Сторінок";
            worksheet.Cell(row, 8).Value = "Інтервал";
        }

        /// <summary>
        /// Формируем в Ексель файле заголовки для тезисов
        /// </summary>
        /// <param name="row"></param>
        /// <param name="worksheet"></param>
        private void AddThesesTitles(int row, IXLWorksheet worksheet)
        {
            worksheet.Cell(row, 1).Value = "№";
            worksheet.Cell(row, 2).Value = "Автор";
            worksheet.Cell(row, 3).Value = "Тезис";
            worksheet.Cell(row, 4).Value = "Конференція";
            worksheet.Cell(row, 5).Value = "Гео";
            worksheet.Cell(row, 6).Value = "Рік";
            worksheet.Cell(row, 7).Value = "Сторінок";
            worksheet.Cell(row, 8).Value = "Інтервал";
        }

        /// <summary>
        /// В зависимости ои наличия в кеше данных по поиску
        /// книг определяем видимость кнопки экспорта
        /// </summary>
        private bool GetBooksDisabled()
        {
            var searchCash = _servBooks.GetSearchCash();
            if (searchCash == null || searchCash.Count == 0) return true;
            else return false;
        }

        /// <summary>
        /// В зависимости ои наличия в кеше данных по поиску
        /// публикаций определяем видимость кнопки экспорта
        /// </summary>
        private bool GetPostsDisabled()
        {
            var searchCash = _servPosts.GetSearchCash();
            if (searchCash == null || searchCash.Count == 0) return true;
            else return false;
        }

        /// <summary>
        /// В зависимости ои наличия в кеше данных по поиск
        /// тезисов определяем видимость кнопки экспорта
        /// </summary>
        private bool GetThesesDisabled()
        {
            var searchCash = _servTheses.GetSearchCash();
            if (searchCash == null || searchCash.Count == 0) return true;
            else return false;
        }
    }
}
