using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class UserProfile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public IEnumerable<UserProfile> Followings { get; set; }

        public IEnumerable<Album> Albums { get; set; }

        public IEnumerable<Picture> Pictures { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
