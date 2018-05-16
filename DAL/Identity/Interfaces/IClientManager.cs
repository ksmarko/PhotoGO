using DAL.Entities;
using System;

namespace DAL.Identity.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(User item);
    }
}
