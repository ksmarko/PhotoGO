using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoGO.DAL.Entities
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }

        public Album()
        {
            Pictures = new List<Picture>();
        }
    }
}
