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
        IUnitOfWork Database { get; set; }

        public AlbumService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public AlbumDTO GetAlbumById(int id)
        {
            var album = Database.Albums.Get(id);

            return Mapper.Map<Album, AlbumDTO>(album);
        }

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
            return true;
        }

        public bool RemoveAlbum(int id)
        {
            var album = Database.Albums.Get(id);

            if (album == null)
                return false;

            Database.Albums.Delete(id);
            return true;
        }

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
            return true;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
