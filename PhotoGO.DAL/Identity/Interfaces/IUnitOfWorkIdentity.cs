using PhotoGO.DAL.Identity.Repositories;
using System;
using System.Threading.Tasks;

namespace PhotoGO.DAL.Identity.Interfaces
{
    public interface IUnitOfWorkIdentity : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }
        Task SaveAsync();
    }
}
