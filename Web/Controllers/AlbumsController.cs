using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class AlbumsController : Controller
    {
        readonly IAlbumService albumService;
        readonly IUserManager userManager;

        public AlbumsController (IAlbumService albumService, IUserManager userManager)
        {
            this.albumService = albumService;
            this.userManager = userManager;
        }

        [Authorize]
        public ActionResult Index(int? page)
        {
            var user = userManager.GetUsers().Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            var albums = albumService.GetAlbumsForUser(user.Id);
            var list = new List<AlbumModel>();
            byte[] defaultImg = System.IO.File.ReadAllBytes(AppContext.BaseDirectory + "Media/album-img.png");

            foreach (var album in albums)
                list.Add(new AlbumModel()
                {
                    Id = album.Id,
                    Name = album.Name,
                    Description = album.Description,
                    Img = album.Pictures.Count > 0 ? album.Pictures.First().Img : defaultImg
                });

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            list.Reverse();

            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        [HttpGet, ActionName("Create")]
        public ActionResult CreateAlbum()
        {
            return PartialView("CreateAlbum");
        }

        [Authorize]
        [HttpPost, ActionName("Create")]
        public ActionResult CreateAlbum(AlbumModel model)
        {
            var user = userManager.GetUsers().Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            var album = new AlbumDTO() { Name = model.Name, Description = model.Description };
            albumService.AddAlbum(album, user.Id);

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet, ActionName("Edit")]
        public ActionResult EditAlbum(int id)
        {
            var album = albumService.GetAlbumById(id);

            var model = new AlbumModel() { Id = album.Id, Name = album.Name, Description = album.Description };
            return PartialView("EditAlbum", model);
        }

        [Authorize]
        [HttpPost, ActionName("Edit")]
        public ActionResult EditAlbumConfirmation(AlbumModel model)
        {
            var albumDto = new AlbumDTO()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };

            albumService.EditAlbum(albumDto);

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet, ActionName("Remove")]
        public ActionResult RemoveAlbum(int id)
        {
            var album = albumService.GetAlbumById(id);
            var model = new AlbumModel() { Id = album.Id, Name = album.Name, Description = album.Description };
            return PartialView("RemoveAlbum", model);
        }

        [Authorize]
        [HttpPost, ActionName("Remove")]
        public ActionResult RemoveAlbumConfirmation(int id)
        {
            albumService.RemoveAlbum(id);
            return RedirectToAction("Index");
        }
    }
}