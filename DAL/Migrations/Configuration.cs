using DAL.EF;
using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DataContext context)
        {

        }
    }
}
