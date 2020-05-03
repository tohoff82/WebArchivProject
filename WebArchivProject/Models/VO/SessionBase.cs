using Microsoft.AspNetCore.Http;

namespace WebArchivProject.Models.VO
{
    abstract class SessionBase
    {
        private readonly IHttpContextAccessor _accessor;

        internal ISession Session =>
            _accessor.HttpContext.Session;

        public SessionBase(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }
}
