using BLL.Interfaces;
using BLL.Services;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Binding
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAlbumService>().To<AlbumService>();
            Bind<IImageService>().To<ImageService>();
            Bind<IUserManager>().To<UserManager>();
        }
    }
}