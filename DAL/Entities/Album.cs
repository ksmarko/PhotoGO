using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public int? UserId { get; set; }
        public virtual UserProfile User { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }

        public Album()
        {
            Pictures = new List<Picture>();
        }
    }
}
