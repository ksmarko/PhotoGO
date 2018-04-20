using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    class UserRepository : IRepository<UserProfile>
    {
        private DataContext db;

        public UserRepository(DataContext context)
        {
            this.db = context;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserProfile> Find(Func<UserProfile, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public UserProfile Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserProfile> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(UserProfile item)
        {
            throw new NotImplementedException();
        }

        public void Create(UserProfile item)
        {
            db.UserProfiles.Add(item);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
