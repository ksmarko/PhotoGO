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
        public byte[] Img { get; set; }

        [Required(ErrorMessage = "Please enter album name")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}