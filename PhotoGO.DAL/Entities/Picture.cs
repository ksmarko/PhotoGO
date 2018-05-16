using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoGO.DAL.Entities
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }

        public byte[] Img { get; set; }

        public int AlbumId { get; set; }
        public virtual Album Album { get; set; }

        public virtual ICollection<User> FavouritedBy { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public Picture()
        {
            FavouritedBy = new List<User>();
            Tags = new List<Tag>();
        }
    }
}
