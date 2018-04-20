using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<AlbumDTO> Albums { get; set; }
        public ICollection<PictureDTO> LikedPictures { get; set; }

        public UserDTO()
        {
            Albums = new List<AlbumDTO>();
            LikedPictures = new List<PictureDTO>();
        }
    }
}
