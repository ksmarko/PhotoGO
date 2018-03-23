using System.Collections.Generic;
using System.Data.Entity;

using DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.EF
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
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
