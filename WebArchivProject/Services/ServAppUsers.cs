using AutoMapper;

using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Models;
using WebArchivProject.Models.DTO;
using WebArchivProject.Models.VO;

using static WebArchivProject.Helper.StringConstant;

namespace WebArchivProject.Services
{
    class ServAppUsers : IServAppUsers
    {
        private readonly IMapper _mapper;
        private readonly IRepoAppUsers _appUsers;
        private readonly IServUserSession _userSession;
        private readonly PagerSettings _pagerSettings;

        private string SessionRole
            => _userSession.User.Role;


        public ServAppUsers(
            IMapper mapper,
            IRepoAppUsers appUsers,
            IServUserSession userSession,
            IOptions<MySettings> options)
        {
            _mapper = mapper;
            _appUsers = appUsers;
            _userSession = userSession;
            _pagerSettings = options.Value.PagerSettings;
        }

        /// <summary>
        /// Получение объекта пагинации пользователей
        /// </summary>
        public async Task<Paginator<DtoAppUserView>> GetPaginatorAsync(int page)
        {
            var appUsers = await GetAppUsersAsync();
            var paginator = Paginator<DtoAppUserView>.ToList(appUsers, page, _pagerSettings.UsersPerPage);
            paginator.ForContainer = APPUSERS_CONTAINER;
            paginator.ForTable = APPUSERS;

            return SessionRole == ROLE_ADMIN ? paginator : null;

        }

        /// <summary>
        /// Получение объектов пользователей
        /// </summary>
        private async Task<IEnumerable<DtoAppUserView>> GetAppUsersAsync()
        {
            var users = await _appUsers.ToListAsync();
            return _mapper.Map<IEnumerable<DtoAppUserView>>(
                users.OrderBy(u => u.Id).Skip(1));
        }
    }
}
