using System.Collections.Generic;

namespace BLL.DTO
{
    public class PictureDTO
    {
        public int Id { get; set; }
        public byte[] Img { get; set; }
        public int AlbumId { get; set; }
        public ICollection<UserDTO> FavouritedBy { get; set; }
        public ICollection<TagDTO> Tags { get; set; }

        public PictureDTO()
        {
            FavouritedBy = new List<UserDTO>();
            Tags = new List<TagDTO>();
        }
    }
}
