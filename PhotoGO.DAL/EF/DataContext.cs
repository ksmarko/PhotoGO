using System.Data.Entity;
using PhotoGO.DAL.Entities;
using PhotoGO.DAL.Migrations;
using PhotoGO.DAL.Identity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PhotoGO.DAL.EF
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<User> UserProfiles { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Tag> Tags { get; set; }

        static DataContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
            Database.SetInitializer((new DbInitializer()));
        }

        public DataContext() : base() { }

        public DataContext(string connectionString) : base(connectionString) { }
    }
}
