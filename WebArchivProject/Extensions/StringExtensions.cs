using System;
using System.Collections.Generic;
using System.Linq;

using WebArchivProject.Models.DTO;

namespace WebArchivProject.Extensions
{
    public static class StringExtensions
    {
        public static string ToNextId(this string id)
            => string.Concat("_del_row_", id);

        public static string Checked(this string type, string t)
        {
            if (type.Contains(t)) return "checked";
            else return string.Empty;
        }

        public static List<DtoAuthor> ToDtoAuthors(this string[] authors)
        {
            int counter = 4;
            var dtoList = new List<DtoAuthor>();
            var groups = authors.GroupBy(_ => counter++ / 4).Select(v => v.ToArray());
            foreach (string[] group in groups)
            {
                dtoList.Add(new DtoAuthor
                {
                    NameUa = group[0],
                    NameRu = group[1],
                    NameEn = group[2],
                    IsFirst = Convert.ToBoolean(group[3])
                });
            }
            return dtoList;
        }
    }
}
