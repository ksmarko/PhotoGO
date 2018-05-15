using BLL.DTO;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IImageService
    {
        IEnumerable<PictureDTO> GetImages();
        IEnumerable<PictureDTO> GetImages(int albumId);
        IEnumerable<PictureDTO> GetFavouritesForUser(string id);
        PictureDTO GetImageById(int id);

        IEnumerable<PictureDTO> SearchImages(params string[] tags);

        void AddImage(PictureDTO image, int albumId);
        void RemoveImage(int id);
        void LikeImage(int id, string userId);
        bool IsLikedBy(string id, int imgId);

        void AddTags(int imgId, params string[] tags);
        string[] GetTags();

        void Dispose();
    }
}
