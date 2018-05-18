using PhotoGO.BLL.DTO;
using PhotoGO.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhotoGO.BLL.Interfaces
{
    /// <summary>
    /// Service for work with users
    /// </summary>
    public interface IUserManager : IDisposable
    {
        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="userDto">User to create</param>
        /// <returns>Returns operation details</returns>
        Task<OperationDetails> Create(UserDTO userDto);

        /// <summary>
        /// Authenticates user
        /// </summary>
        /// <param name="userDto">User</param>
        /// <returns>Returns claims identity</returns>
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);

        /// <summary>
        /// Gets user by its name
        /// </summary>
        /// <param name="name">User name</param>
        /// <exception cref="UserNotFoundException">When User not found</exception>
        /// <returns>Returns user</returns>
        UserDTO GetUserByName(string name);

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>Returns list of users</returns>
        IEnumerable<UserDTO> GetUsers();

        /// <summary>
        /// Gets all roles
        /// </summary>
        /// <returns>Returns list of roles</returns>
        IEnumerable<string> GetRoles();

        /// <summary>
        /// Edit user roles
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="newRoleName">Name of new role</param>
        /// <exception cref="UserNotFoundException">When user not found</exception>
        /// <returns></returns>
        Task EditRole(string userId, string newRoleName);
    }
}
