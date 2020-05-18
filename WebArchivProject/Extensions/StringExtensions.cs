using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Extensions
{
    /// <summary>
    /// Методы расширения
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Создание идентификатора удаляемого тега
        /// </summary>
        public static string ToNextId(this string id)
            => string.Concat("_del_row_", id);

        /// <summary>
        /// Проверка на содержание "checked" в строке
        /// </summary>
        public static string Checked(this string type, string t)
        {
            if (type.Contains(t)) return "checked";
            else return string.Empty;
        }

        /// <summary>
        /// Преобразование массива строк в список объектов для транфера
        /// </summary>
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

        /// <summary>
        /// преобразование города и издательства в формат "Д.:Город"
        /// </summary>
        public static string ToIssuerLine(this string city, string issuer)
            => new StringBuilder().AppendFormat("{0}.:{1}", city[0], issuer)
                .ToString();

        /// <summary>
        /// Преобразование списка объектов в список строк
        /// </summary>
        public static List<string> NextAuthors(this List<Author> authors)
        {
            if (authors.Count > 1)
                return authors.Skip(1)
                    .Select(l => l.NameUa)
                    .ToList();
            else return new List<string>();
        }

        /// <summary>
        /// Преобразование строки интервала страниц в количество страниц
        /// </summary>
        public static int ToCount(this string interval)
        {
            string[] arr = interval.Split(" — ");
            return int.Parse(arr[1]) - int.Parse(arr[0]) + 1;
        }

        /// <summary>
        /// Преобразование города и Страны в одну строку
        /// </summary>
        public static string ToLocate(this string country, string city)
            => new StringBuilder(country).Append($",\n\r{city}").ToString();

        /// <summary>
        /// Преобразование данных пагинатора в специальный идентификатор пагинации
        /// </summary>
        public static string ToNav(this int count, string nav, string target)
            => string.Format("{0}_{1}_{2}", count, nav, target);

        /// <summary>
        /// Преобразование сточного идентификатора действия в число
        /// </summary>
        public static int ToPageNum(this string action)
        {
            string[] arr = action.Split('_');
            if (arr[1] == "next") return int.Parse(arr[0]) + 1;
            if (arr[1] == "prev") return int.Parse(arr[0]) - 1;
            else return int.Parse(arr[0]);
        }

        /// <summary>
        /// Преобразование строки с именами авторов (на трех языках) в строку с именем на Украинском
        /// </summary>
        public static string ToNameUa(this string filterName)
            => filterName.Split('/')[0];

        /// <summary>
        /// Преобразование списка авторов в строку для ячейки эксель
        /// </summary>
        public static string ToXLCell(this List<string> authors)
        {
            if (authors == null || authors.Count == 0) return string.Empty;
            else return string.Join(", ", authors);
        }

        /// <summary>
        /// Создаем идентификатор выезжающей области редактирования
        /// </summary>
        public static string ToCollapseId(this int id, string itemType, string target, bool withHash = false)
        {
            if (withHash) return string.Format($"#collapse_{id}_{itemType}_{target}");
            else return string.Format($"collapse_{id}_{itemType}_{target}");
        }

        /// <summary>
        /// Создаем идентификатор области редактирования
        /// </summary>
        public static string ToEditedId(this int id, string itemType, string target)
            => string.Format($"edited_{id}_{itemType}_{target}");

        /// <summary>
        /// Получаем цель взаимодействия
        /// </summary>
        public static string ToTarget(this string tableType)
            => tableType.Split('_')[2];

        /// <summary>
        /// Получаем идентификатор цели взаимодействия
        /// </summary>
        public static int ToItemId(this string tableType)
            => int.Parse(tableType.Split('_')[1]);
    }
}
