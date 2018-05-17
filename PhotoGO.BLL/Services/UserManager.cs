using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using PhotoGO.BLL.DTO;
using PhotoGO.BLL.Infrastructure;
using PhotoGO.BLL.Interfaces;
using PhotoGO.DAL.Entities;
using PhotoGO.DAL.Identity.Entities;
using PhotoGO.DAL.Identity.Interfaces;
using PhotoGO.DAL.Interfaces;
using Microsoft.AspNet.Identity;

namespace PhotoGO.BLL.Services
{
    /// <summary>
    /// Service for work with users
    /// </summary>
    public class UserManager : IUserManager
    {
        /// <summary>
        /// Represents database for identity
        /// </summary>
        IUnitOfWorkIdentity DatabaseIdentity { get; set; }

        /// <summary>
        /// Represents domain database
        /// </summary>
        IUnitOfWork DatabaseDomain { get; set; }

        /// <summary>
        /// Creates service
        /// </summary>
        /// <param name="uowi">Identity UnitOfWork</param>
        /// <param name="uow">UnitOfWork</param>
        public UserManager(IUnitOfWorkIdentity uowi, IUnitOfWork uow)
        {
            DatabaseIdentity = uowi;
            DatabaseDomain = uow;
        }

        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="userDto">User to create</param>
        /// <returns>Returns operation details</returns>
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

        /// <summary>
        /// Authenticates user
        /// </summary>
        /// <param name="userDto">User</param>
        /// <returns>Returns claims identity</returns>
        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await DatabaseIdentity.UserManager.FindAsync(userDto.Email, userDto.Password);

            if (user != null)
                claim = await DatabaseIdentity.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        /// <summary>
        /// Gets user by its name
        /// </summary>
        /// <param name="name">User name</param>
        /// <returns>Returns user</returns>
        public UserDTO GetUserByName(string name)
        {
            var appUser = DatabaseIdentity.UserManager.FindByName(name);
            return CreateUserDTO(appUser);
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>Returns list of users</returns>
        public IEnumerable<UserDTO> GetUsers()
        {
            var appUsers = DatabaseIdentity.UserManager.Users;
            var list = new List<UserDTO>();

            foreach (var appUser in appUsers)
                list.Add(CreateUserDTO(appUser));

            return list;
        }

        /// <summary>
        /// Edit user roles
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="newRoleName">Name of new role</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets all roles
        /// </summary>
        /// <returns>Returns list of roles</returns>
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
        private UserDTO CreateUserDTO(ApplicationUser user)
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
