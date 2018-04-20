using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Services;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMediaService>().To<MediaService>();
            Bind<IUserManager>().To<UserManager>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BLL.Infrastructure.AutoMapperConfig.Initialize();

            NinjectModule serviceModule = new ConnectionModule(
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            NinjectModule module = new ServiceModule();
            var kernel = new StandardKernel(serviceModule, module);

            kernel.Get(typeof(PhotoAlbumApp));
        }
    }
}
