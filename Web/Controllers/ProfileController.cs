using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class ProfileController : Controller
    {
        readonly IMediaService mediaService;
        readonly IUserManager userManager;

        public ProfileController (IMediaService ms, IUserManager um)
        {
            mediaService = ms;
            userManager = um;
        }

        public ActionResult Index()
        {
            return View("UserProfile");
        }

        public ActionResult Albums(int? page)
        {
            var user = userManager.GetUsers().Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            var albums = mediaService.GetAlbumsForUser(user.Id);
            var list = new List<AlbumModel>();
            byte[] defaultImg = System.IO.File.ReadAllBytes(AppContext.BaseDirectory + "Media/album-img.png");

            foreach (var album in albums)
                list.Add(new AlbumModel()
                {
                    Name = album.Name,
                    Description = album.Description,
                    Img = album.Pictures.Count > 0 ? album.Pictures.ElementAt(new Random().Next(0, album.Pictures.Count)).Img : defaultImg
                });

            int pageSize = 8;
            int pageNumber = (page ?? 1);

            return PartialView("Albums", list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Favourites()
        {
            return Content("likes are here");
        }

        public ActionResult Settings()
        {
            return Content("this is settings");
        }

        [HttpGet]
        public ActionResult CreateAlbum()
        {
            return PartialView("CreateAlbum");
        }

        [HttpPost]
        public ActionResult CreateAlbum(AlbumModel model)
        {
            var user = userManager.GetUsers().Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            var album = new AlbumDTO() { Name = model.Name, Description = model.Description };
            mediaService.AddAlbum(album, user.Id);

            return RedirectToAction("Index");
        }
    }
}