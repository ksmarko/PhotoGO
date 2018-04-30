using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class PictureDTO
    {
        public int Id { get; set; }
        public byte[] Img { get; set; }
        public AlbumDTO Album { get; set; }
        public ICollection<UserDTO> FavouritedBy { get; set; }
        public ICollection<TagDTO> Tags { get; set; }

        public PictureDTO()
        {
            FavouritedBy = new List<UserDTO>();
            Tags = new List<TagDTO>();
        }
    }
}
