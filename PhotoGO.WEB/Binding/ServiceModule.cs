using Ninject.Modules;
using PhotoGO.BLL.Interfaces;
using PhotoGO.BLL.Services;

namespace PhotoGO.WEB.Binding
{
    /// <summary>
    /// Binding for BLL services
    /// </summary>
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