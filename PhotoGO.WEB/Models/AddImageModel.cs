using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PhotoGO.WEB.Models
{
    public class AddImageModel
    {
        [Required(ErrorMessage = "Please select file (-s)")]
        public HttpPostedFileBase[] Files { get; set; }

        [RegularExpression("(\\w+\\s?)+", ErrorMessage = "Please input tags in format tag1 tag2 tag3")]
        public string Tags { get; set; }
    }
}