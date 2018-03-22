using System.Data.Entity;

using DAL.Entities;

namespace DAL.EF
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        static DataContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataContext>());
        }

        public DataContext(string connectionString) : base(connectionString)
        {

        }
    }
}
