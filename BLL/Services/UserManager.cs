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
        IUnitOfWorkIdentity DatabaseIdentity { get; set; }
        IUnitOfWork DatabaseDomain { get; set; }

        public UserManager(IUnitOfWorkIdentity uowi, IUnitOfWork uow)
        {
            DatabaseIdentity = uowi;
            DatabaseDomain = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            var user = await DatabaseIdentity.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                var result = await DatabaseIdentity.UserManager.CreateAsync(user, userDto.Password);

                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                await DatabaseIdentity.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                User clientProfile = new User { Id = user.Id, Name = userDto.Name };
                DatabaseIdentity.ClientManager.Create(clientProfile);
                await DatabaseIdentity.SaveAsync();
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
            ApplicationUser user = await DatabaseIdentity.UserManager.FindAsync(userDto.Email, userDto.Password);

            if (user != null)
                claim = await DatabaseIdentity.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public UserDTO GetUserById(string id)
        {
            var appUser = DatabaseIdentity.UserManager.FindById(id);

            var user = new UserDTO()
            {
                Id = appUser.Id,
                Email = appUser.Email,
                UserName = appUser.UserName,
                Name = appUser.User.Name,
                Role = SelectRoleNameById(appUser.Roles.Where(x => x.UserId == appUser.Id).Single().RoleId),
                Albums = Mapper.Map<IEnumerable<Album>, ICollection<AlbumDTO>>(DatabaseDomain.Albums.Find(x => x.User.Id == id)), 
                LikedPictures = Mapper.Map<ICollection<Picture>, ICollection<PictureDTO>>(DatabaseDomain.Users.Get(id).LikedPictures)
            };

            return user;
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var appUsers = DatabaseIdentity.UserManager.Users;
            var list = new List<UserDTO>();

            foreach (var el in appUsers)
            {
                list.Add(new UserDTO()
                {
                    Id = el.Id,
                    Email = el.Email,
                    UserName = el.UserName,
                    Name = el.User.Name,
                    Role = SelectRoleNameById(el.Roles.Where(x => x.UserId == el.Id).Single().RoleId),
                    Albums = Mapper.Map<IEnumerable<Album>, ICollection<AlbumDTO>>(DatabaseDomain.Albums.Find(x => x.User.Id == el.Id)),
                    LikedPictures = Mapper.Map<ICollection<Picture>, ICollection<PictureDTO>>(DatabaseDomain.Users.Get(el.Id).LikedPictures)
                });
            }

            return list;
        }

        public void RemoveFromRole(string userId, string oldRoleName)
        {
            var user = DatabaseIdentity.UserManager.FindById(userId);
            DatabaseIdentity.UserManager.RemoveFromRole(userId, oldRoleName);
            DatabaseIdentity.UserManager.Update(user);
        }

        public void AddToRole(string userId, string roleName)
        {
            var user = DatabaseIdentity.UserManager.FindById(userId);
            DatabaseIdentity.UserManager.AddToRole(userId, roleName);
            DatabaseIdentity.UserManager.Update(user);
        }

        private string SelectRoleNameById(string id)
        {
            var appRoles = DatabaseIdentity.RoleManager.Roles;
            return appRoles.Where(x => x.Id == id).Single().Name;
        }

        public IEnumerable<string> GetRoles()
        {
            return DatabaseIdentity.RoleManager.Roles.Select(x => x.Name);
        }

        public void Dispose()
        {
            DatabaseIdentity.Dispose();
        }
    }
}
