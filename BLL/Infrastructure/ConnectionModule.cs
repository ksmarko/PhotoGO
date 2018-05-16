using PhotoGO.DAL.Identity.Interfaces;
using PhotoGO.DAL.Identity.Repositories;
using PhotoGO.DAL.Interfaces;
using PhotoGO.DAL.Repositories;
using Ninject.Modules;

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
