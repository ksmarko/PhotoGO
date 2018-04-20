using BLL.Interfaces;
using DAL.Identity;
using DAL.Identity.Interfaces;
using DAL.Identity.Repositories;
using DAL.Interfaces;
using DAL.Repositories;

using Ninject.Modules;
using System.Net;

namespace BLL.Infrastructure
{
    public class ConnectionModule : NinjectModule
    {
        private string connectionString;

        public ConnectionModule(string connection)
        {
            connectionString = connection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
            Bind<IUnitOfWorkIdentity>().To<IdentityUnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}
