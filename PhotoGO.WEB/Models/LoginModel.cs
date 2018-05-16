using System.ComponentModel.DataAnnotations;

namespace PhotoGO.WEB.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please, enter email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please, enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}