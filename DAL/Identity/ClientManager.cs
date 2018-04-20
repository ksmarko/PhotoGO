using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Identity
{
    public class ClientManager : IClientManager
    {
        public DataContext Database { get; set; }
        public ClientManager(DataContext db)
        {
            Database = db;
        }

        public void Create(UserProfile item)
        {
            Database.UserProfiles.Add(item);
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
