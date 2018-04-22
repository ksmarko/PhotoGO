using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    class PhotoAlbumApp
    {
        readonly IMediaService ms;
        readonly IUserManager um;

        public PhotoAlbumApp(IMediaService service, IUserManager userManager)
        {
            this.ms = service;
            this.um = userManager;
            Run();
        }

        public async void Run()
        {
            #region ...
            //var album1 = new AlbumDTO()
            //{

            //    Name = "My first album",
            //    Description = "Album1 for testing my awesome app",
            //    UserId = user.Id
            //};

            //var album2 = new AlbumDTO()
            //{
            //    Name = "My second album",
            //    Description = "Album2 for testing my awesome app",
            //    UserId = user.Id
            //};

            //ms.AddAlbum(album1);
            //ms.AddAlbum(album2);

            //foreach (var el in ms.GetAlbums())
            //    Console.WriteLine(string.Join(" ", el.Id, el.Name, el.Description));

            //ms.AddImage(new PictureDTO() { Img = new byte[] { Convert.ToByte(12) } }, 1);
            //ms.AddImage(new PictureDTO() { Img = new byte[] { Convert.ToByte(4) } }, 1);
            //ms.AddImage(new PictureDTO() { Img = new byte[] { Convert.ToByte(66) } }, 1);

            //ms.LikeImage(1, "1");
            //Console.WriteLine("Edited");

            //var e = ms.GetAlbumById(1);
            //e.Name = "Edited name";

            //ms.EditAlbum(e);

            //foreach (var el in ms.GetAlbums())
            //    Console.WriteLine(string.Join(" ", el.Id, el.Name, el.Description));
            #endregion
            Console.ReadKey();
        }
    }
}