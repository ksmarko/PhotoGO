using System.Collections.Generic;

namespace PhotoGO.WEB.Models
{
    public class ImageModel
    {
        public int Id { get; set; }

        public byte[] Img { get; set; }

        public int Likes { get; set; }

        public ICollection<string> Tags { get; set; }

        public bool IsLiked { get; set; }

        public bool IsMy { get; set; }

        public ImageModel()
        {
            Tags = new List<string>();
        }
    }
}