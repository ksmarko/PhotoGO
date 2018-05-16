using System.ComponentModel.DataAnnotations;

namespace PhotoGO.WEB.Models
{
    public class TagsModel
    {
        [Required(ErrorMessage = "Please input tags")]
        [RegularExpression("(\\w+\\s?)+", ErrorMessage = "Please input tags in format tag1 tag2 tag3")]
        public string Tags { get; set; }
    }
}