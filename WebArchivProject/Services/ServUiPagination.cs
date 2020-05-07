using System.Collections.Generic;

using WebArchivProject.Contracts;
using WebArchivProject.Models.VO;

namespace WebArchivProject.Services
{
    public class ServUiPagination : IServUiPagination
    {
        // to do implement states for paginator settings
        private readonly byte _uiPaginatorSize = 4;

        public string NextDisable(IPaginator paginator)
            => !paginator.HasNext ? "disabled" : null;

        public string PrevDisable(IPaginator paginator)
            => !paginator.HasPrevious ? "disabled" : null;

        public string CurrDisable(IPaginator paginator, int num)
            => paginator.CurrentPage == num ? "disabled" : null;

        public SortedDictionary<int, string> NavBlock(IPaginator paginator)
        {
            var pVal = new PagerLine
            (
                size: _uiPaginatorSize,
                current: paginator.CurrentPage,
                total: paginator.TotalPages
            );

            var temp = new SortedDictionary<int, string>();
            foreach (int item in pVal.PageArr)
            {
                temp.Add(item, NavActive(paginator.CurrentPage == item));
            }
            return temp;
        }

        private string NavActive(bool isActive)
            => isActive ? "active" : null;
    }
}
