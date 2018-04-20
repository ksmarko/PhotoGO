using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Album> Albums { get; set; }

        public virtual ICollection<Picture> LikedPictures { get; set; }

        public UserProfile()
        {
            LikedPictures = new List<Picture>();
            Albums = new List<Album>();
        }
    }
}
