using System.ComponentModel.DataAnnotations;

namespace PhotoGO.WEB.Models
{
    public class AlbumModel
    {
        public int Id { get; set; }

        public byte[] Img { get; set; }

        [Required(ErrorMessage = "Please enter album name")]
        [MaxLength(100, ErrorMessage = "Name must be no longer 100 symbols")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(700, ErrorMessage = "Description must be no longer 700 symbols")]
        public string Description { get; set; }
    }
}