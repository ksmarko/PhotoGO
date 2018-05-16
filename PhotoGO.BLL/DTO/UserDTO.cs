using System.Collections.Generic;

namespace PhotoGO.BLL.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<AlbumDTO> Albums { get; set; }
        public ICollection<PictureDTO> LikedPictures { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

        public UserDTO()
        {
            Albums = new List<AlbumDTO>();
            LikedPictures = new List<PictureDTO>();
        }
    }
}
