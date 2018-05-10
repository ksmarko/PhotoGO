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
    public class ImageService : IImageService
    {
        IUnitOfWork Database { get; set; }

        public ImageService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<PictureDTO> GetImages(int albumId)
        {
            var list = Database.Pictures.Find(x => x.AlbumId == albumId);

            if (list == null)
                throw new ArgumentNullException();

            var res = new List<PictureDTO>();

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
                img.Tags.Add(Database.Tags.Get(GetTag(tag)));

            Database.Pictures.Create(img);
            Database.Save();
        }

        private int GetTag(TagDTO entity)
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
                img.Tags.Add(Database.Tags.Get(GetTag(new TagDTO { Name = tag })));

            Database.Pictures.Update(img);
            Database.Save();
        }

        public PictureDTO GetImageById(int id)
        {
            var img = Database.Pictures.Get(id);

            if (img == null)
                throw new ArgumentNullException();

            return Mapper.Map<Picture, PictureDTO>(img);
        }

        public IEnumerable<PictureDTO> SearchImages(params string[] tags)
        {
            if (tags == null)
                throw new ArgumentNullException();

            var pictures = new List<Picture>();

            foreach (var tag in tags)
            {
                if (string.IsNullOrWhiteSpace(tag))
                    continue;

                var t = Database.Tags.Find(x => x.Name == tag.Trim()).FirstOrDefault();

                if (t == null)
                {
                    pictures.Clear();
                    break;
                }

                if (pictures.Count <= 0)
                    pictures = t.Pictures.ToList();
                else
                    pictures = (t.Pictures).Intersect(pictures).ToList();
            }

            return Mapper.Map<IEnumerable<Picture>, IEnumerable<PictureDTO>>(pictures);
        }

        public string[] GetTags()
        {
            var tags = Database.Tags.GetAll().Select(x => x.Name).ToArray();
            return tags;
        }

        public IEnumerable<PictureDTO> GetImages()
        {
            return Mapper.Map<IEnumerable<Picture>, IEnumerable<PictureDTO>>(Database.Pictures.GetAll());
        }

        public IEnumerable<PictureDTO> GetFavouritesForUser(string id)
        {
            var user = Database.Users.Get(id);

            if (user == null)
                throw new ArgumentNullException();

            return Mapper.Map<IEnumerable<Picture>, IEnumerable<PictureDTO>>(user.LikedPictures);
        }

        public bool IsLikedBy(string id, int imgId)
        {
            if (Database.Users.Get(id).LikedPictures.Any(x => x.Id == imgId))
                return true;

            return false;
        }

        public void LikeImage(int id, string userId)
        {
            var img = Database.Pictures.Get(id);
            var user = Database.Users.Get(userId);

            if (img == null | user == null)
                throw new ArgumentNullException();

            if (img.FavouritedBy.Any(x => x.Id == user.Id))
                img.FavouritedBy.Remove(user);
            else img.FavouritedBy.Add(user);

            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
