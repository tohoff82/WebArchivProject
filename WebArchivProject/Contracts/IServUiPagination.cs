using System.Collections.Generic;

namespace WebArchivProject.Contracts
{
    public interface IServUiPagination
    {
        string PrevDisable(IPaginator paginator);
        string NextDisable(IPaginator paginator);
        string CurrDisable(IPaginator paginator, int num);
        SortedDictionary<int, string> NavBlock(IPaginator paginator);
    }
}
