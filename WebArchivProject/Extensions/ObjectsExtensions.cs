using System.Collections.Generic;

using WebArchivProject.Models.ArchivDb;

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
    }
}
