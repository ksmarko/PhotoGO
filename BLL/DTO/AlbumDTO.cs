using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class AlbumDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public ICollection<PictureDTO> Pictures { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public AlbumDTO()
        {
            Pictures = new List<PictureDTO>();
        }
    }
}
