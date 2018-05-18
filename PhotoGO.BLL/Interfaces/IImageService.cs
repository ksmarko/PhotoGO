using System;
using System.Collections.Generic;
using PhotoGO.BLL.DTO;

namespace PhotoGO.BLL.Interfaces
{
    /// <summary>
    /// Service for work with Images and tags
    /// </summary>
    public interface IImageService : IDisposable
    {
        /// <summary>
        /// Gets all images
        /// </summary>
        /// <returns>Returns list of images</returns>
        IEnumerable<PictureDTO> GetImages();

        /// <summary>
        /// Gets images from album
        /// </summary>
        /// <param name="albumId">Album id</param>
        /// <returns>Returns list of images</returns>
        IEnumerable<PictureDTO> GetImages(int albumId);

        /// <summary>
        /// Search images by tags
        /// </summary>
        /// <param name="tags">set of tags</param>
        /// <returns>Returns list of images with specified tags</returns>
        IEnumerable<PictureDTO> SearchImages(params string[] tags);

        /// <summary>
        /// Gets image by its id
        /// </summary>
        /// <param name="id">Image id</param>
        /// <exception cref="TargetNotFoundException">When image not found</exception>
        PictureDTO GetImageById(int id);

        /// <summary>
        /// Adds image into album
        /// </summary>
        /// <param name="item">Image</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="TargetNotFoundException">When album not found</exception>
        void AddImage(PictureDTO image);

        /// <summary>
        /// Removes image from album
        /// </summary>
        /// <param name="id">Image id</param>
        /// <exception cref="TargetNotFoundException">When image not found</exception>
        void RemoveImage(int id);

        /// <summary>
        /// Likes or dislikes image by user
        /// </summary>
        /// <param name="id">Image id</param>
        /// <param name="userId">User id</param>
        /// <exception cref="UserNotFoundException">When user not found</exception>
        /// <exception cref="TargetNotFoundException">When image not found</exception>
        void LikeImage(int id, string userId);

        /// <summary>
        /// If image is liked by user
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="imgId">Image id</param>
        /// <exception cref="UserNotFoundException">When user not found</exception>
        /// <returns>Returns true if image is liked by user and false if isn't</returns>
        bool IsLikedBy(string id, int imgId);

        /// <summary>
        /// Adds tags to the image
        /// </summary>
        /// <param name="imgId">Image id</param>
        /// <param name="tags">Set of tags</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="TargetNotFoundException">When image not found</exception>
        void AddTags(int imgId, params string[] tags);

        /// <summary>
        /// Gets all tags
        /// </summary>
        /// <returns>Returns array of tags</returns>
        string[] GetTags();
    }
}
