using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection.Emit;
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
            Database.SetInitializer((new DropCreateDatabaseAlways<DataContext>()));
        }

        public DataContext(string connectionString) : base(connectionString) {}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>().HasRequired(a => a.User).WithMany(p => p.Albums);
            modelBuilder.Entity<Picture>().HasRequired(a => a.Album).WithMany(p => p.Pictures);
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
    }
}
