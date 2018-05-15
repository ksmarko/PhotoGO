using BLL.DTO;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IAlbumService
    {
        IEnumerable<AlbumDTO> GetAlbumsForUser(string userId);
        AlbumDTO GetAlbumById(int id);

        void AddAlbum(AlbumDTO item, string userId);
        void RemoveAlbum(int id);
        void EditAlbum(AlbumDTO item);

        void Dispose();
    }
}
