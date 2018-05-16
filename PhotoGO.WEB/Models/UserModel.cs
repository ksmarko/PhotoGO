using System.ComponentModel;

namespace PhotoGO.WEB.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [DisplayName("E-mail")]
        public string UserName { get; set; }

        public string Role { get; set; }
    }
}