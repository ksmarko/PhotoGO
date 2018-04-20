using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }
        public byte[] Img { get; set; }
        public int AlbumId { get; set; }
        public virtual Album Album { get; set; }
        public virtual ICollection<UserProfile> FavouritedBy { get; set; }
        public ICollection<string> Tags { get; set; }

        public Picture()
        {
            FavouritedBy = new List<UserProfile>();
            Tags = new List<string>();
        }
    }
}
