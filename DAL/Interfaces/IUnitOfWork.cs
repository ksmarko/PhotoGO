using System;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Album> Albums { get; }
        IRepository<Picture> Pictures { get; }
        IRepository<UserProfile> Users { get; }
        void Save();
    }
}
