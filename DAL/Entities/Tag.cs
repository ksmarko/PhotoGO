using System.Collections.Generic;

namespace DAL.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }

        public Tag()
        {
            Pictures = new List<Picture>();
        }
    }
}
