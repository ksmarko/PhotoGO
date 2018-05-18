using System;
using System.Collections.Generic;
using AutoMapper;
using PhotoGO.BLL.DTO;
using PhotoGO.BLL.Enums;
using PhotoGO.BLL.Exceptions;
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
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="uow">UnitOfWork</param>
        public AlbumService(IUnitOfWork uow)
        {
            Database = uow ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Search album in database by id
        /// </summary>
        /// <param name="id">Album id</param>
        /// <exception cref="TargetNotFoundException">When album not found</exception>
        public AlbumDTO GetAlbumById(int id)
        {
            var album = Database.Albums.Get(id);

            if (album == null)
                throw new TargetNotFoundException(Target.Album);

            return Mapper.Map<Album, AlbumDTO>(album);
        }

        /// <summary>
        /// Creates new album
        /// </summary>
        /// <param name="item">Album</param>
        /// <exception cref="ArgumentNullException">When input item is null</exception>
        /// <exception cref="UserNotFoundException">When user not found</exception>
        public void AddAlbum(AlbumDTO item)
        {
            if (item == null)
                throw new ArgumentNullException();

            var user = Database.Users.Get(item.UserId);

            if (user == null)
                throw new UserNotFoundException();

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
        }

        /// <summary>
        /// Removes album and its images from database
        /// </summary>
        /// <param name="id">Album id</param>
        /// <exception cref="TargetNotFoundException">When album not found</exception>
        public void RemoveAlbum(int id)
        {
            var album = Database.Albums.Get(id);

            if (album == null)
                throw new TargetNotFoundException(Target.Album);

            Database.Albums.Delete(id);
            Database.Save();
        }

        /// <summary>
        /// Shanges album name and/or description
        /// </summary>
        /// <param name="item">Album with new data</param>
        /// <exception cref="TargetNotFoundException">When album not found</exception>
        public void EditAlbum(AlbumDTO item)
        {
            if (item == null)
                throw new ArgumentNullException();

            var album = Database.Albums.Get(item.Id);

            if (album == null)
                throw new TargetNotFoundException(Target.Album);

            album.Name = item.Name;
            album.Description = item.Description;

            Database.Albums.Update(album);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
