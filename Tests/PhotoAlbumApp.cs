using BLL.Interfaces;
using System;

namespace Tests
{
    class PhotoAlbumApp
    {
        readonly IMediaService ms;

        public PhotoAlbumApp(IMediaService service)
        {
            this.ms = service;
            Run();
        }

        public void Run()
        {
            foreach (var el in ms.GetAlbums())
                Console.WriteLine(el.Name + " " + el.Pictures);
        }
    }
}