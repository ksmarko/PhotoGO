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
        void AddAlbum(AlbumDTO album);
        void RemoveAlbum(AlbumDTO album);
        void EditAlbum(AlbumDTO album);

        IEnumerable<PictureDTO> GetImages(AlbumDTO album);
        void AddImage(PictureDTO image, AlbumDTO album);
        void RemoveImage(PictureDTO image);
        void AddTags(params string [] tags);
        void LikeImage(PictureDTO picture);

        void Dispose();
    }
}
