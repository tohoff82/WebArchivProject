using System.Collections.Generic;
using System.Linq;
using System.Text;

using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Extensions
{
    public static class ObjectsExtensions
    {
        public static List<Author> With(this List<Author> authors, string guid)
        {
            foreach (var author in authors)
            {
                author.ExternalId = guid;
            }
            return authors;
        }

        public static string ToFilterName(this IGrouping<string, Author> authors)
        {
            var sb = new StringBuilder(authors.Key);
            var authorRu = authors.FirstOrDefault(a => a.NameRu != null);
            var authorEn = authors.FirstOrDefault(a => a.NameEn != null);
            if (authorRu != null) sb.AppendFormat("/{0}", authorRu.NameRu);
            if (authorEn != null) sb.AppendFormat("/{0}", authorEn.NameEn);
            return sb.ToString();
        }

        public static List<DtoSearchresultBook> DtoBookFilterByAuthor (this List<DtoSearchresultBook> searchresultBooks, string authorName)
        {
            var currentBooks = new List<DtoSearchresultBook>();
            foreach (var srBook in searchresultBooks)
            {
                bool firstContains = srBook.AuthorFirst.Contains(authorName);
                bool nextContains = srBook.AuthorsNext != null ? srBook.AuthorsNext.Contains(authorName) : false;
                if (firstContains || nextContains) currentBooks.Add(srBook);
            }
            return currentBooks;
        }

        public static List<DtoSearchresultPost> DtoPostFilterByAuthor(this List<DtoSearchresultPost> searchresultPosts, string authorName)
        {
            var currentPosts = new List<DtoSearchresultPost>();
            foreach (var srPost in searchresultPosts)
            {
                if (srPost.Authors.Contains(authorName)) currentPosts.Add(srPost);
            }
            return currentPosts;
        }

        public static List<DtoSearchresultThesis> DtoThesisFilterByAuthor(this List<DtoSearchresultThesis> searchresultTheses, string authorName)
        {
            var currentThesis = new List<DtoSearchresultThesis>();
            foreach (var srThesis in searchresultTheses)
            {
                if (srThesis.Authors.Contains(authorName)) currentThesis.Add(srThesis);
            }
            return currentThesis;
        }
    }
}
