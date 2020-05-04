using AutoMapper;

using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Models;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Services
{
    /// <summary>
    /// Сервис, отвечающий за авторизацию и регистрацию пользователя
    /// </summary>
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

        /// <summary>
        /// Метод авторизации пользователя
        /// </summary>
        /// <param name="loginUser">Объект данных из формы ввода</param>
        /// <returns>Объект ответа</returns>
        public async Task<DtoInterlayerIdentity> LoginAsync(DtoFormLoginUser loginUser)
        {
            var user = await _appUsers.GetAppUserByEmailAsync(loginUser.Mail);

            if (user == null) return new DtoInterlayerIdentity
            {
                IsSuccess = false,
                Reason = "Такий емеіл не зараєстровано"
            };

            if (user != null && user.Password != loginUser.Password)
                return new DtoInterlayerIdentity
                {
                    IsSuccess = false,
                    Reason = "Введено не вірний пароль"
                };

            else
            {
                CreateSession(user);
                return new DtoInterlayerIdentity
                {
                    IsSuccess = true,
                    Reason = "{0}, вас успішно авторизовано!"
                };
            }
        }

        /// <summary>
        /// Метод регистрации новго пользователя
        /// </summary>
        /// <param name="registerUser">Объект данных из формы ввода</param>
        /// <returns>Объект ответа</returns>
        public async Task<DtoInterlayerIdentity> RegisterAsync(DtoFormRegisterUser registerUser)
        {
            await _appUsers.AddAsync(_mapper.Map<AppUser>(registerUser));
            return new DtoInterlayerIdentity
            {
                IsSuccess = true,
                Reason = "{0}, ваш обліковий запис успішно створено!"
            };
        }

        /// <summary>
        /// Метод создания сессии
        /// </summary>
        /// <param name="appUser">Объект юзера из БД</param>
        private void CreateSession(AppUser appUser)
        {
            _userSession.UpdateUserSession(_mapper.Map<SessionUser>(appUser));
        }
    }
}
