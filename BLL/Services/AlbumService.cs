using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    public class AlbumService : IAlbumService
    {
        /// <summary>
        /// Represents an entry point for domain repositories
        /// </summary>
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Service constructor
        /// </summary>
        /// <param name="uow"></param>
        public AlbumService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public AlbumDTO GetAlbumById(int id)
        {
            var album = Database.Albums.Get(id);

            if (album == null)
                throw new ArgumentNullException();
            
            return Mapper.Map<Album, AlbumDTO>(album);
        }

        public void AddAlbum(AlbumDTO item)
        {
            var user = Database.Users.Get(item.UserId);

            if (item == null || user == null)
                throw new ArgumentNullException();

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

        public void RemoveAlbum(int id)
        {
            var album = Database.Albums.Get(id);

            if (album == null)
                throw new ArgumentNullException();

            Database.Albums.Delete(id);
        }

        public void EditAlbum(AlbumDTO item)
        {
            if (item == null)
                throw new ArgumentNullException();

            var album = Database.Albums.Get(item.Id);

            if (album == null)
                throw new ArgumentNullException();

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
