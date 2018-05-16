using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Identity.Entities;
using DAL.Identity.Interfaces;
using DAL.Interfaces;
using Microsoft.AspNet.Identity;

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
                return new OperationDetails(true, "Success register", "");
            }
            else
                return new OperationDetails(false, "User with this login is already exist", "Email");
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await DatabaseIdentity.UserManager.FindAsync(userDto.Email, userDto.Password);

            if (user != null)
                claim = await DatabaseIdentity.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public UserDTO GetUserByName(string name)
        {
            var appUser = DatabaseIdentity.UserManager.FindByName(name);
            return CreateUser(appUser);
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var appUsers = DatabaseIdentity.UserManager.Users;
            var list = new List<UserDTO>();

            foreach (var appUser in appUsers)
                list.Add(CreateUser(appUser));

            return list;
        }

        public async Task EditRole(string userId, string newRoleName)
        {
            var user = await DatabaseIdentity.UserManager.FindByIdAsync(userId);
            var oldRole = GetRoleForUser(userId);

            if (oldRole != newRoleName)
            {
                await DatabaseIdentity.UserManager.RemoveFromRoleAsync(userId, oldRole);
                await DatabaseIdentity.UserManager.AddToRoleAsync(userId, newRoleName);

                await DatabaseIdentity.UserManager.UpdateAsync(user);
            }
        }

        public IEnumerable<string> GetRoles()
        {
            return DatabaseIdentity.RoleManager.Roles.Select(x => x.Name);
        }

        public void Dispose()
        {
            DatabaseIdentity.Dispose();
            DatabaseDomain.Dispose();
        }

        #region Private methods

        private UserDTO CreateUser(ApplicationUser user)
        {
            return new UserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Name = user.User.Name,
                Role = GetRoleForUser(user.Id),
                Albums = Mapper.Map<IEnumerable<Album>, ICollection<AlbumDTO>>(DatabaseDomain.Albums.Find(x => x.User.Id == user.Id)),
                LikedPictures = Mapper.Map<ICollection<Picture>, ICollection<PictureDTO>>(DatabaseDomain.Users.Get(user.Id).LikedPictures)
            };
        }

        private string GetRoleForUser(string id)
        {
            var user = DatabaseIdentity.UserManager.FindById(id);
            var roleId = user.Roles.Where(x => x.UserId == user.Id).Single().RoleId;
            var role = DatabaseIdentity.RoleManager.Roles.Where(x => x.Id == roleId).Single().Name;

            return role;
        }
        #endregion
    }
}
