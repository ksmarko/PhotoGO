using System.Collections.Generic;
using System.Data.Entity;

using DAL.Entities;
using DAL.Identity;
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
            Database.SetInitializer(new DbInitializer());
        }

        public DataContext(string connectionString) : base(connectionString)
        {

        }
    }

    public class DbInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            context.Roles.Add(new IdentityRole() { Name = "admin" });
            context.Roles.Add(new IdentityRole() { Name = "user" });

            context.Users.Add(new ApplicationUser()
            {
                UserName = "ks_marko",
                Email = "ksunia.sergienko@gmail.com",
                PasswordHash = "ABr5je2Iv7yEjs6f2yO4PZXzH+BCktjvPZ5S391cFXz2F1zlMwRtyaDDWS51MQYHIw==", //"password"
                EmailConfirmed = true,
                //Roles = ??????????????????????????

                UserProfile = new UserProfile()
                {
                    Name = "Ksenia Marko",
                    Albums = new List<Album>(),
                    Followings = new List<UserProfile>(),
                    Pictures = new List<Picture>()
                }
            });
            context.SaveChanges();
        }
    }
}
