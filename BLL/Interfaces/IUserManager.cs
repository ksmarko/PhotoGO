using BLL.DTO;
using BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserManager : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);

        UserDTO GetUserByName(string name);
        IEnumerable<UserDTO> GetUsers();

        IEnumerable<string> GetRoles();
        Task EditRole(string userId, string newRoleName);
    }
}
