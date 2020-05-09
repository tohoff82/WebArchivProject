using System.Threading.Tasks;

using WebArchivProject.Models.DTO;
using WebArchivProject.Models.SearchFilters;
using WebArchivProject.Models.VO;

namespace WebArchivProject.Contracts
{
    public interface IServPosts
    {
        Task AddToDbAsync(DtoPost dtoPost);
        Task DeleteFromDbAsync(int postId);
        PostsComboFilters GetPostsComboFilters();
        Paginator<DtoSearchresultPost> GetPaginatorResultModal(PostsSearchFilter filter);
        Paginator<DtoSearchresultPost> GetPaginationResult(int pageNumber, int pageSize, string target);
        Paginator<DtoSearchresultPost> GetPostsSearchPaginator(int pageNumber, int pageSize, string target);
    }
}
