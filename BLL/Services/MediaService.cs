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
    public class MediaService : IMediaService
    {
        IUnitOfWork Database { get; set; }

        public MediaService(IUnitOfWork uow)
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

        public IEnumerable<PictureDTO> GetImages(int albumId)
        {
            var list = Database.Pictures.Find(x => x.AlbumId == albumId);
            var res = new List<PictureDTO>();
            if (list == null)
                throw new ArgumentNullException();

            foreach (var el in list)
                res.Add(new PictureDTO()
                {
                    Id = el.Id,
                    Img = el.Img,
                    Album = Mapper.Map<Album, AlbumDTO>(el.Album),
                    FavouritedBy = Mapper.Map<ICollection<User>, ICollection<UserDTO>>(el.FavouritedBy),
                    Tags = Mapper.Map<ICollection<Tag>, ICollection<TagDTO>>(el.Tags)
                });

            return res;
        }

        public void AddImage(PictureDTO item, int albumId) //edit? (albumId, tags)
        {
            var album = Database.Albums.Get(albumId);

            if (item == null | album == null)
                throw new ArgumentNullException();

            var img = new Picture()
            {
                Album = album,
                AlbumId = album.Id,
                Img = item.Img,
            };

            foreach (var tag in item.Tags)
                img.Tags.Add(Database.Tags.Get(AddTag(tag)));

            Database.Pictures.Create(img);
            Database.Save();
        }

        private int AddTag(TagDTO entity)
        {
            var tag = Database.Tags.Find(x => x.Name == entity.Name).FirstOrDefault();

            if (tag == null)
                Database.Tags.Create(new Tag() { Name = entity.Name });

            Database.Save();
            return Database.Tags.Find(x => x.Name == entity.Name).FirstOrDefault().Id;
        }

        public void RemoveImage(int id)
        {
            var img = Database.Pictures.Get(id);

            if (img != null)
                Database.Pictures.Delete(id);
        }

        public void AddTags(int imgId, params string[] tags)
        {
            var img = Database.Pictures.Get(imgId);

            if (img == null)
                throw new ArgumentNullException();

            img.Tags.Clear();

            foreach (var tag in tags)
                img.Tags.Add(Database.Tags.Get(AddTag(new TagDTO { Name = tag })));

            Database.Pictures.Update(img);
            Database.Save();
        }

        public void LikeImage(int id, string userId) //maybe edit (remove userId)
        {
            var img = Database.Pictures.Get(id);
            var user = Database.Users.Get(userId);

            if (img == null | user == null)
                throw new ArgumentNullException();

            img.FavouritedBy.Add(user);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public AlbumDTO GetAlbumById(int id)
        {
            var album = Database.Albums.Get(id);

            if (album == null)
                throw new ArgumentNullException();

            return Mapper.Map<Album, AlbumDTO>(album);
        }

        public PictureDTO GetImageById(int id)
        {
            var img = Database.Pictures.Get(id);

            if (img == null)
                throw new ArgumentNullException();

            return Mapper.Map<Picture, PictureDTO>(img);
        }

        public IEnumerable<PictureDTO> SearchImages(string tag)
        {
            var t = Database.Tags.Find(x => x.Name == tag).FirstOrDefault();
            return Mapper.Map<IEnumerable<Picture>, IEnumerable<PictureDTO>>(t.Pictures);
        }

        public IEnumerable<PictureDTO> GetImages()
        {
            return Mapper.Map<IEnumerable<Picture>, IEnumerable<PictureDTO>>(Database.Pictures.GetAll());
        }
    }
}
