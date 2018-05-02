using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AlbumService : IAlbumService
    {
        IUnitOfWork Database { get; set; }

        public AlbumService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<AlbumDTO> GetAlbumsForUser(string userId)
        {
            var user = Database.Users.Get(userId);

            if (user == null)
                throw new ArgumentNullException();

            return Mapper.Map<IEnumerable<Album>, IEnumerable<AlbumDTO>>(Database.Albums.GetAll().Where(x => x.UserId == userId));
        }

        public AlbumDTO GetAlbumById(int id)
        {
            var album = Database.Albums.Get(id);

            if (album == null)
                throw new ArgumentNullException();

            return Mapper.Map<Album, AlbumDTO>(album);
        }

        public void AddAlbum(AlbumDTO item, string userId)
        {
            var user = Database.Users.Get(userId);

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
