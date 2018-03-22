using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<User> Followings { get; set; }

        public IEnumerable<Album> Albums { get; set; }

        public IEnumerable<Picture> Pictures { get; set; }
    }
}
