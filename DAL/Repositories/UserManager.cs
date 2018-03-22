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
    class UserManager : IUserManager
    {
        //set private?
        private DataContext db { get; set; }

        public UserManager(DataContext context)
        {
            this.db = context;
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
