using System.Collections.Generic;

namespace BLL.DTO
{
    public class TagDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PictureDTO> Pictures { get; set; }

        public TagDTO()
        {
            Pictures = new List<PictureDTO>();
        }
    }
}
