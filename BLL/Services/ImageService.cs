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
                return null;

            var res = new List<PictureDTO>();

            foreach (var el in list)
                res.Add(new PictureDTO()
                {
                    Id = el.Id,
                    Img = el.Img,
                    AlbumId = el.AlbumId,
                    FavouritedBy = Mapper.Map<ICollection<User>, ICollection<UserDTO>>(el.FavouritedBy),
                    Tags = Mapper.Map<ICollection<Tag>, ICollection<TagDTO>>(el.Tags)
                });

            return res;
        }

        public bool AddImage(PictureDTO item)
        {
            if (item == null)
                return false;

            var album = Database.Albums.Get(item.AlbumId);

            if (album == null)
                return false;

            var img = new Picture()
            {
                Album = album,
                AlbumId = album.Id,
                Img = item.Img,
            };

            foreach (var tag in item.Tags)
                img.Tags.Add(Database.Tags.Get(GetTag(tag.Name)));

            Database.Pictures.Create(img);
            Database.Save();
            return true;
        }
        
        public bool RemoveImage(int id)
        {
            var img = Database.Pictures.Get(id);

            if (img == null)
                return false;

            Database.Pictures.Delete(id);
            return true;
        }

        public bool AddTags(int imgId, params string[] tags)
        {
            var img = Database.Pictures.Get(imgId);

            if (img == null || tags == null)
                return false;

            img.Tags.Clear();

            foreach (var tag in tags)
                if (string.IsNullOrWhiteSpace(tag))
                    continue;
                else
                    img.Tags.Add(Database.Tags.Get(GetTag(tag.Trim())));

            Database.Pictures.Update(img);
            Database.Save();
            return true;
        }

        public PictureDTO GetImageById(int id)
        {
            var img = Database.Pictures.Get(id);            
            return Mapper.Map<Picture, PictureDTO>(img);
        }

        public IEnumerable<PictureDTO> SearchImages(params string[] tags)
        {
            var pictures = new List<Picture>();

            if (tags == null)
                return new List<PictureDTO>();

            foreach (var tag in tags)
            {
                if (string.IsNullOrWhiteSpace(tag))
                    continue;

                var t = Database.Tags.Find(x => x.Name.ToLower() == tag.ToLower()).FirstOrDefault();

                if (t == null || t.Pictures.Count == 0) //if tag do not exist or do not have images
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

        public bool IsLikedBy(string id, int imgId)
        {
            if (Database.Users.Get(id).LikedPictures.Any(x => x.Id == imgId))
                return true;

            return false;
        }

        public bool LikeImage(int id, string userId)
        {
            var img = Database.Pictures.Get(id);
            var user = Database.Users.Get(userId);

            if (img == null | user == null)
                return false;

            if (img.FavouritedBy.Any(x => x.Id == user.Id))
                img.FavouritedBy.Remove(user);
            else img.FavouritedBy.Add(user);

            Database.Save();
            return true;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        private int GetTag(string name)
        {
            name = name.ToLower();
            var tag = Database.Tags.Find(x => x.Name.ToLower() == name).FirstOrDefault();

            if (tag == null)
                Database.Tags.Create(new Tag() { Name = name });

            Database.Save();
            return Database.Tags.Find(x => x.Name == name).FirstOrDefault().Id;
        }
    }
}
