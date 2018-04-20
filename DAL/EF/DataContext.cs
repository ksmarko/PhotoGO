using System.Data.Entity;
using DAL.Entities;

namespace DAL.EF
{
    public class DataContext : DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        static DataContext()
        {
        }

        public DataContext(string connectionString) : base(connectionString)
        {

        }
    }
}
