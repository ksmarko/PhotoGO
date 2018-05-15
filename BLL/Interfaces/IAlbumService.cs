using BLL.DTO;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IAlbumService
    {
        AlbumDTO GetAlbumById(int id);

        void AddAlbum(AlbumDTO item);
        void RemoveAlbum(int id);
        void EditAlbum(AlbumDTO item);

        void Dispose();
    }
}
