using System;

using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Album> Albums { get; }
        IRepository<Picture> Pictures { get; }
        void Save();
    }
}
