using System;
using System.Collections.Generic;
using System.Linq;

using WebArchivProject.Contracts;

namespace WebArchivProject.Models.VO
{
    public class Paginator<T> : List<T>, IPaginator
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public string ForTable { get; set; }
        public string ForContainer { get; set; }
        public string Size { get; set; }

        public Paginator(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static Paginator<T> ToList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            int count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new Paginator<T>(items, count, pageNumber, pageSize);
        }
    }
}
