using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class AlbumModel
    {
        public int Id { get; set; }
        public byte[] Img { get; set; }

        [Required(ErrorMessage = "Please enter album name")]
        public string Name { get; set; }

        [MaxLength(200, ErrorMessage = "Description must be no longer 200 symbols")]
        public string Description { get; set; }
    }
}