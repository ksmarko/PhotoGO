using AutoMapper;
using PhotoGO.BLL.DTO;
using PhotoGO.BLL.Interfaces;
using PhotoGO.DAL.Entities;
using PhotoGO.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PhotoGO.BLL.Services
{
    /// <summary>
    /// Service for work with Images and tags
    /// </summary>
    public class ImageService : IImageService
    {
        /// <summary>
        /// Represents domain database
        /// </summary>
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Creates service
        /// </summary>
        /// <param name="uow">UnitOfWork</param>
        public ImageService(IUnitOfWork uow)
        {
            Database = uow;
        }

        /// <summary>
        /// Gets images from album
        /// </summary>
        /// <param name="albumId">album id</param>
        /// <returns>Returns list of images</returns>
        public IEnumerable<PictureDTO> GetImages(int albumId)
        {
            var list = Database.Pictures.Find(x => x.AlbumId == albumId);
            var res = new List<PictureDTO>();

            if (list == null)
                return res;            

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

        /// <summary>
        /// Adds image into album
        /// </summary>
        /// <param name="item">Image</param>
        /// <returns>Returns true if operation successfully completed and false if doesn't</returns>
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

        /// <summary>
        /// Removes image from album
        /// </summary>
        /// <param name="id">Image id</param>
        /// <returns>Returns true if operation successfully completed and false if doesn't</returns>
        public bool RemoveImage(int id)
        {
            var img = Database.Pictures.Get(id);

            if (img == null)
                return false;

            Database.Pictures.Delete(id);
            return true;
        }

        /// <summary>
        /// Adds tags to the image
        /// </summary>
        /// <param name="imgId">Image id</param>
        /// <param name="tags">Set of tags</param>
        /// <returns>Returns true if operation successfully completed and false if doesn't</returns>
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

        /// <summary>
        /// Gets image by its id
        /// </summary>
        /// <param name="id">Image id</param>
        /// <returns>Returns image if it exist or null if doesn't</returns>
        public PictureDTO GetImageById(int id)
        {
            var img = Database.Pictures.Get(id);            
            return Mapper.Map<Picture, PictureDTO>(img);
        }

        /// <summary>
        /// Search images by tags
        /// </summary>
        /// <param name="tags">set of tags</param>
        /// <returns>Returns list of images with specified tags</returns>
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

                //if tag do not exist or do not have images
                if (t == null || t.Pictures.Count == 0) 
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

        /// <summary>
        /// Gets all tags
        /// </summary>
        /// <returns>Returns array of tags</returns>
        public string[] GetTags()
        {
            var tags = Database.Tags.GetAll().Select(x => x.Name).ToArray();
            return tags;
        }

        /// <summary>
        /// Gets all images
        /// </summary>
        /// <returns>Returns list of images</returns>
        public IEnumerable<PictureDTO> GetImages()
        {
            return Mapper.Map<IEnumerable<Picture>, IEnumerable<PictureDTO>>(Database.Pictures.GetAll());
        }

        /// <summary>
        /// If image is liked by user
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="imgId">Image id</param>
        /// <returns>Returns true if image is liked by user and false if isn't</returns>
        public bool IsLikedBy(string id, int imgId)
        {
            if (Database.Users.Get(id).LikedPictures.Any(x => x.Id == imgId))
                return true;

            return false;
        }

        /// <summary>
        /// Likes or dislikes image by user
        /// </summary>
        /// <param name="id">Image id</param>
        /// <param name="userId">User id</param>
        /// <returns>Returns true if operation successfully completed and false if doesn't</returns>
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

        #region Private methods
        private int GetTag(string name)
        {
            name = name.ToLower();
            var tag = Database.Tags.Find(x => x.Name.ToLower() == name).FirstOrDefault();

            if (tag == null)
                Database.Tags.Create(new Tag() { Name = name });

            Database.Save();
            return Database.Tags.Find(x => x.Name == name).FirstOrDefault().Id;
        }
        #endregion
    }
}
