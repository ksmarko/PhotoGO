using PhotoGO.DAL.Identity.Entities;
using System.Data.Entity;

namespace PhotoGO.DAL.EF
{
    //TEMP
    public class DbInitializer : CreateDatabaseIfNotExists<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            var userRole = new ApplicationRole { Name = "user" };
            var moderatorRole = new ApplicationRole { Name = "moderator" };
            var adminRole = new ApplicationRole { Name = "admin" };

            context.Roles.Add(userRole);
            context.Roles.Add(moderatorRole);
            context.Roles.Add(adminRole);

            context.SaveChanges();
        }
    }
}
