using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public byte[] Img { get; set; }
        public string Path { get; set; }
        public int Likes { get; set; }
        public ICollection<string> Tags { get; set; }
    }
}