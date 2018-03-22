using System;
using System.Threading.Tasks;
using DAL.EF;
using DAL.Entities;
using DAL.Identity;
using DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private DataContext db;
        private AlbumRepository albumRepository;
        private PictureRepository pictureRepository;

        private ApplicationUserManager appUserManager;
        private ApplicationRoleManager roleManager;
        private IUserManager userProfileManager;

        public EFUnitOfWork(string connectionString)
        {
            db = new DataContext(connectionString);
            appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            userProfileManager = new UserManager(db);
        }

        public ApplicationUserManager AppUserManager
        {
            get { return appUserManager; }
        }

        public IUserManager UserProfileManager
        {
            get { return userProfileManager; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public IRepository<Album> Albums
        {
            get
            {
                if (albumRepository == null)
                    albumRepository = new AlbumRepository(db);
                return albumRepository;
            }
        }

        public IRepository<Picture> Pictures
        {
            get
            {
                if (pictureRepository == null)
                    pictureRepository = new PictureRepository(db);
                return pictureRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
