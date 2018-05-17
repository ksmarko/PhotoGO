using System.Collections.Generic;
using AutoMapper;
using PhotoGO.BLL.DTO;
using PhotoGO.BLL.Interfaces;
using PhotoGO.DAL.Entities;
using PhotoGO.DAL.Interfaces;

namespace PhotoGO.BLL.Services
{
    /// <summary>
    /// Service for work with albums
    /// </summary>
    public class AlbumService : IAlbumService
    {
        /// <summary>
        /// Represents domain database
        /// </summary>
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Creates service
        /// </summary>
        /// <param name="uow">UnitOfWork</param>
        public AlbumService(IUnitOfWork uow)
        {
            Database = uow;
        }

        /// <summary>
        /// Search album in database by id
        /// </summary>
        /// <param name="id">Album id</param>
        /// <returns>Returns album if it exist or null if doesn't</returns>
        public AlbumDTO GetAlbumById(int id)
        {
            var album = Database.Albums.Get(id);

            return Mapper.Map<Album, AlbumDTO>(album);
        }

        /// <summary>
        /// Creates new album
        /// </summary>
        /// <param name="item">Album</param>
        /// <returns>Returns true if operation successfully completed and false if doesn't</returns>
        public bool AddAlbum(AlbumDTO item)
        {
            if (item == null)
                return false;

            var user = Database.Users.Get(item.UserId);

            if (user == null)
                return false;

            var album = new Album()
            {
                Name = item.Name,
                Description = item.Description,
                UserId = user.Id,
                User = user,
                Pictures = Mapper.Map<ICollection<PictureDTO>, ICollection<Picture>>(item.Pictures)
            };

            Database.Albums.Create(album);
            Database.Save();
            return true;
        }

        /// <summary>
        /// Removes album and its images from database
        /// </summary>
        /// <param name="id">Album id</param>
        /// <returns>Returns true if operation successfully completed and false if doesn't</returns>
        public bool RemoveAlbum(int id)
        {
            var album = Database.Albums.Get(id);

            if (album == null)
                return false;

            Database.Albums.Delete(id);
            Database.Save();
            return true;
        }

        /// <summary>
        /// Shanges album name and/or description
        /// </summary>
        /// <param name="item">Album with new data</param>
        /// <returns>Returns true if operation successfully completed and false if doesn't</returns>
        public bool EditAlbum(AlbumDTO item)
        {
            if (item == null)
                return false;

            var album = Database.Albums.Get(item.Id);

            if (album == null)
                return false;

            album.Name = item.Name;
            album.Description = item.Description;

            Database.Albums.Update(album);
            Database.Save();
            return true;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
