using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IMediaService
    {
        IEnumerable<AlbumDTO> GetAlbums();
        AlbumDTO GetAlbumById(int id);
        void AddAlbum(AlbumDTO item);
        void RemoveAlbum(int id);
        void EditAlbum(AlbumDTO item);

        IEnumerable<PictureDTO> GetImages(int albumId);
        void AddImage(PictureDTO image, int albumId);
        void RemoveImage(int id);
        void AddTags(int imgId, params string [] tags);
        void LikeImage(int id, int userId);
        PictureDTO GetImageById(int id);

        void Dispose();
    }
}
