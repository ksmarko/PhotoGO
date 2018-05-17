using PhotoGO.DAL.EF;
using System.Data.Entity.Migrations;

namespace PhotoGO.DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DataContext context) { }
    }
}
