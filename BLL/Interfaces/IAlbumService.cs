using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
