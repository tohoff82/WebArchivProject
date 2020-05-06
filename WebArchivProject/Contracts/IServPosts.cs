﻿using System.Threading.Tasks;

using WebArchivProject.Models.DTO;
using WebArchivProject.Models.VO;

namespace WebArchivProject.Contracts
{
    public interface IServPosts
    {
        Task AddToDbAsync(DtoPost dtoPost);
        Paginator<DtoSearchresultPost> GetPaginationResult(int pageNumber, int pageSize);
    }
}