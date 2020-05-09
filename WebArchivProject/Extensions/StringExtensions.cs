using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WebArchivProject.Models.ArchivDb;
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

        public static string ToIssuerLine(this string city, string issuer)
            => new StringBuilder().AppendFormat("{0}.:{1}", city[0], issuer)
                .ToString();

        public static List<string> NextAuthors(this List<Author> authors)
        {
            if (authors.Count > 1)
                return authors.Skip(1)
                    .Select(l => l.NameUa)
                    .ToList();
            else return new List<string>();
        }

        public static int ToCount(this string interval)
        {
            string[] arr = interval.Split(" — ");
            return int.Parse(arr[1]) - int.Parse(arr[0]) + 1;
        }

        public static string ToLocate(this string country, string city)
            => new StringBuilder(country).Append($",\n\r{city}").ToString();

        public static string ToNav(this int count, string nav, string target)
            => string.Format("{0}_{1}_{2}", count, nav, target);

        public static int ToPageNum(this string action)
        {
            string[] arr = action.Split('_');
            if (arr[1] == "next") return int.Parse(arr[0]) + 1;
            if (arr[1] == "prev") return int.Parse(arr[0]) - 1;
            else return int.Parse(arr[0]);
        }

        public static string ToNameUa(this string filterName)
            => filterName.Split('/')[0];

        public static string ToXLCell(this List<string> authors)
        {
            if (authors == null || authors.Count == 0) return string.Empty;
            else return string.Join(", ", authors);
        }
    }
}
