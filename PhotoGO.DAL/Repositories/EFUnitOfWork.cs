using System;
using PhotoGO.DAL.EF;
using PhotoGO.DAL.Entities;
using PhotoGO.DAL.Interfaces;

namespace PhotoGO.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private DataContext db;
        private GenericRepository<Album> albumRepository;
        private GenericRepository<Picture> pictureRepository;
        private GenericRepository<Tag> tagsRepository;
        private UserRepository userRepository;

        public EFUnitOfWork(string connectionString)
        {
            db = new DataContext(connectionString);
        }

        public IRepository<Album> Albums
        {
            get
            {
                if (albumRepository == null)
                    albumRepository = new GenericRepository<Album>(db);
                return albumRepository;
            }
        }

        public IRepository<Picture> Pictures
        {
            get
            {
                if (pictureRepository == null)
                    pictureRepository = new GenericRepository<Picture>(db);
                return pictureRepository;
            }
        }

        public IRepository<Tag> Tags
        {
            get
            {
                if (tagsRepository == null)
                    tagsRepository = new GenericRepository<Tag>(db);
                return tagsRepository;
            }
        }

        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        #region Dispose
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
        #endregion
    }
}
