using Microsoft.AspNetCore.Http;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;
using WebArchivProject.Models;
using WebArchivProject.Models.VO;

using static WebArchivProject.Helper.StringConstant;

namespace WebArchivProject.Services
{
    class ServUserSession : SessionBase, IServUserSession
    {
        /// <summary>
        /// Получаем текущего пользователя из сессии
        /// </summary>
        public SessionUser User
            => Session.GetObj<SessionUser>(SESSION_USER);

        public ServUserSession(IHttpContextAccessor accessor)
            : base(accessor) { }

        /// <summary>
        /// Удаляем Сессию
        /// </summary>
        public void RemoveUserSession()
            => Session.Remove(SESSION_USER);

        /// <summary>
        /// Обновляем сессию
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUserSession(SessionUser user)
        {
            Session.Remove(SESSION_USER);
            Session.SetObj(SESSION_USER, user);
        }
    }
}
