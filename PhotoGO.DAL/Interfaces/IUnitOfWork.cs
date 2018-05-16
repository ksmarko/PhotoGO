using System;
using PhotoGO.DAL.Entities;

namespace PhotoGO.DAL.Interfaces
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
