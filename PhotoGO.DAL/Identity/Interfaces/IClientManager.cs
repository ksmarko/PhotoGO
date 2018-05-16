using PhotoGO.DAL.Entities;
using System;

namespace PhotoGO.DAL.Identity.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(User item);
    }
}
