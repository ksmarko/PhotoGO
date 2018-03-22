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
        public IEnumerable<UserDTO> FavouritedBy { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
