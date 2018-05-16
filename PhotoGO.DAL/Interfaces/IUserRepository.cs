using PhotoGO.DAL.Entities;
using System;
using System.Collections.Generic;

namespace PhotoGO.DAL.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User Get(string id);
        void Create(User item);
        void Update(User item);
        void Delete(string id);
        IEnumerable<User> Find(Func<User, bool> predicate);
    }
}
