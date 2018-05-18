using PhotoGO.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoGO.WEB.Helpers
{
    public class FileHelper
    {
        public static void SetDefaultImage(ref ICollection<AlbumModel> list)
        {
            string path = AppContext.BaseDirectory + "favicon.ico";

            if (System.IO.File.Exists(path))
            {
                byte[] defaultImg = System.IO.File.ReadAllBytes(path);
                list.Where(x => x.Img.Length <= 0).ToList().ForEach(x => x.Img = defaultImg);
            }
        }
    }
}