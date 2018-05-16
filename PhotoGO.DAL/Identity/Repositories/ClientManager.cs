using PhotoGO.DAL.EF;
using PhotoGO.DAL.Entities;
using PhotoGO.DAL.Identity.Interfaces;

namespace PhotoGO.DAL.Identity.Repositories
{
    public class ClientManager : IClientManager
    {
        public DataContext Database { get; set; }

        public ClientManager(DataContext db)
        {
            Database = db;
        }

        public void Create(User item)
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
