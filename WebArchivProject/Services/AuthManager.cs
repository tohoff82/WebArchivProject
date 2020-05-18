using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Options;
using System;
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
        private readonly int _sessDuration;

        public AuthManager(
            IMapper mapper,
            IRepoAppUsers appUsers,
            IServUserSession userSession,
            IOptions<SecurityCreds> options)
        {
            _mapper = mapper;
            _appUsers = appUsers;
            _userSession = userSession;
            _sessDuration = options.Value.SessionDuration;
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
            if (await AppUserMailHasExist(registerUser.Mail)) return new DtoInterlayerIdentity
            {
                IsSuccess = false,
                Reason = $"{registerUser.Name}, така поштова скринька вже зареєстрована"
            };

            await _appUsers.AddAsync(_mapper.Map<AppUser>(registerUser));
            await LoginAsync(_mapper.Map<DtoFormLoginUser>(registerUser));
            return new DtoInterlayerIdentity
            {
                IsSuccess = true,
                Reason = "{0}, ваш обліковий запис успішно створено!"
            };
        }

        /// <summary>
        /// Метод редактирования данных пользователя
        /// </summary>
        public async Task<DtoInterlayerIdentity> EditUserAsync(DtoFormEditUser editUser)
        {
            var appUser = await _appUsers.GetAppUserByIdAsync(_userSession.User.Id);
            _mapper.Map(editUser, appUser);
            await _appUsers.UpdateUserAsync(appUser);
            CreateSession(appUser);
            return new DtoInterlayerIdentity
            {
                IsSuccess = true
            };
        }

        private async Task<bool> AppUserMailHasExist(string mail)
        {
            var user = await _appUsers.GetAppUserByEmailAsync(mail);
            return user != null ? true : false;
        }

        /// <summary>
        /// Метод создания сессии
        /// </summary>
        /// <param name="appUser">Объект юзера из БД</param>
        private void CreateSession(AppUser appUser)
        {
            var sessionUser = _mapper.Map<SessionUser>(appUser);
            sessionUser.Expirate = DateTimeOffset.UtcNow
                .AddMinutes(_sessDuration).ToUnixTimeMilliseconds();

            _userSession.UpdateUserSession(sessionUser);
        }
    }
}
