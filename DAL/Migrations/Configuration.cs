using DAL.EF;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.EF.DataContext context)
        {

        }

    }
}
