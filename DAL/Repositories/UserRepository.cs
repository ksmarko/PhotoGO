using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        public DataContext Database { get; set; }

        public UserRepository(DataContext db)
        {
            Database = db;
        }

        public IEnumerable<User> GetAll()
        {
            return Database.UserProfiles;
        }

        public User Get(string id)
        {
            return Database.UserProfiles.Find(id);
        }

        public void Create(User item)
        {
            Database.UserProfiles.Add(item);
        }

        public void Update(User item)
        {
            Database.Entry(item).State = EntityState.Modified;
        }

        public void Delete(string id)
        {
            User user = Database.UserProfiles.Find(id);
            if (user != null)
                Database.UserProfiles.Remove(user);
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return Database.UserProfiles.Where(predicate).ToList();
        }
    }
}
