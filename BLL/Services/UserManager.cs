using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Identity;
using DAL.Identity.Entities;
using DAL.Identity.Interfaces;
using DAL.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserManager : IUserManager
    {
        IUnitOfWorkIdentity Database { get; set; }
        IUnitOfWork Data { get; set; }

        public UserManager(IUnitOfWorkIdentity uowi, IUnitOfWork uow)
        {
            Database = uowi;
            Data = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            var user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                // добавляем роль
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                // создаем профиль клиента
                User clientProfile = new User { Id = user.Id, Name = userDto.Name };
                Database.ClientManager.Create(clientProfile);
                //Data.Users.Create(clientProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        // начальная инициализация бд
        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }

        public UserDTO GetUserById(string id)
        {
            var user = Data.Users.Get(id);
            return Mapper.Map<User, UserDTO>(user);
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            return Mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(Data.Users.GetAll());
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
