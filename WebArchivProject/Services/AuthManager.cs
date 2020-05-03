using AutoMapper;

using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Models;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Services
{
    class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly IRepoAppUsers _appUsers;
        private readonly IServUserSession _userSession;

        public AuthManager(
            IMapper mapper,
            IRepoAppUsers appUsers,
            IServUserSession userSession)
        {
            _mapper = mapper;
            _appUsers = appUsers;
            _userSession = userSession;
        }

        public async Task<DtoInterlayerLogin> LoginAsync(DtoFormLoginUser loginUser)
        {
            var user = await _appUsers.GetAppUserByEmailAsync(loginUser.Mail);

            if (user == null) return new DtoInterlayerLogin
            {
                IsSuccess = false,
                Reason = "Такий емеіл не зараєстровано"
            };

            if (user != null && user.Password != loginUser.Password)
                return new DtoInterlayerLogin
                {
                    IsSuccess = false,
                    Reason = "Введено не вірний пароль"
                };

            else
            {
                CreateSession(user);
                return new DtoInterlayerLogin
                {
                    IsSuccess = true,
                    Reason = "Вас успішно авторизовано!"
                };
            }
        }

        public Task RegisterAsync(DtoFormRegisterUser registerUser)
            => _appUsers.AddAsync(_mapper.Map<AppUser>(registerUser));

        private void CreateSession(AppUser appUser)
        {
            _userSession.UpdateUserSession(_mapper.Map<SessionUser>(appUser));
        }
    }
}
