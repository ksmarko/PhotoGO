using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }

        public byte[] Img { get; set; }

        public IEnumerable<UserProfile> FavouritedBy { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
