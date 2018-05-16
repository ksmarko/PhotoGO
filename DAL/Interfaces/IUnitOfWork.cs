using System;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Album> Albums { get; }
        IRepository<Picture> Pictures { get; }
        IRepository<Tag> Tags { get; }
        IUserRepository Users { get; }
        void Save();
    }
}
