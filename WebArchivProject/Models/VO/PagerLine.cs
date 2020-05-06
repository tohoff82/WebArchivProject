using System.Collections.Generic;

namespace WebArchivProject.Models.VO
{
    public class PagerLine
    {
        private readonly int _total;
        private readonly int _current;
        private readonly int _size;

        public List<int> PageArr => CurrentArr();

        public PagerLine(int total, int current, int size)
        {
            _total = total;
            _current = current;
            _size = size;
        }

        private List<int> CurrentArr()
        {
            bool isEven = _size.IsEven(out int offset);

            if (_total < _size)
                return ConstructArr(1, _total);

            if (_current.CheckLeft(offset, out int start) &&
               (_current + offset).CheckRight(_total, isEven))
                return ConstructArr(start, _size);

            if (start < 0 || start == 0)
                return ConstructArr(1, _size);

            else return ConstructArr(_total - _size + 1, _size);
        }

        private List<int> ConstructArr(int start, int pagerSize)
        {
            var list = new List<int>();
            for (int i = start; i < start + pagerSize; i++)
            {
                list.Add(i);
            }
            return list;
        }
    }

    public static class PvExt
    {
        public static bool IsEven(this int pagerSize, out int sideOffset)
        {
            sideOffset = pagerSize / 2;
            return pagerSize % 2 == 0 ? true : false;
        }

        public static bool CheckLeft(this int currentPage, int sideOffset, out int startPage)
        {
            startPage = currentPage - sideOffset;
            return startPage > 0 ? true : false;
        }

        public static bool CheckRight(this int calcEnd, int totalPage, bool isEven)
        {
            int endPage = isEven ? calcEnd - 1 : calcEnd;
            return (endPage < totalPage || endPage == totalPage) ? true : false;
        }
    }
}
