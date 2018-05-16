using BLL.DTO;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IImageService
    {
        IEnumerable<PictureDTO> GetImages();
        IEnumerable<PictureDTO> GetImages(int albumId);
        IEnumerable<PictureDTO> SearchImages(params string[] tags);
        PictureDTO GetImageById(int id);
        
        bool AddImage(PictureDTO image);
        bool RemoveImage(int id);
        bool LikeImage(int id, string userId);
        bool IsLikedBy(string id, int imgId);

        bool AddTags(int imgId, params string[] tags);
        string[] GetTags();

        void Dispose();
    }
}
