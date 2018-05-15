using System.Collections.Generic;

namespace BLL.DTO
{
    public class AlbumDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public ICollection<PictureDTO> Pictures { get; set; }

        public AlbumDTO()
        {
            Pictures = new List<PictureDTO>();
        }
    }
}
