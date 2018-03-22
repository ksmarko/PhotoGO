using System;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Identity;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Album> Albums { get; }
        IRepository<Picture> Pictures { get; }
        void Save();

        ApplicationUserManager AppUserManager { get; }
        IUserManager UserProfileManager { get; }
        ApplicationRoleManager RoleManager { get; }
        Task SaveAsync();
    }
}
