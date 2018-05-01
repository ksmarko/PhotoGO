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
    public class MediaController : Controller
    {
        readonly IMediaService mediaService;
        readonly IUserManager userManager;

        public MediaController (IMediaService ms, IUserManager um)
        {
            mediaService = ms;
            userManager = um;
        }

        public ActionResult Search(string tag, int? page)
        {
            var images = mediaService.SearchImages(tag);
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            ViewBag.IsSearchResult = true;
            ViewBag.Tag = tag;
            var list = new List<ImageModel>();

            foreach (var img in images)
            {
                list.Add(new ImageModel() { Id = img.Id, Img = img.Img, Likes = img.FavouritedBy.Count, Tags = img.Tags.Select(x => x.Name).ToList() });
            }

            return View("Images", list.ToPagedList(pageNumber, pageSize));
        }

        #region Images
        public ActionResult Images(int albumId, int? page)
        {
            var imgs = mediaService.GetImages(albumId).ToList();
            var list = new List<ImageModel>();

            foreach (var img in imgs)
            {
                list.Add(new ImageModel() { Id = img.Id, Img = img.Img, Likes = img.FavouritedBy.Count, Tags = img.Tags.Select(x => x.Name).ToList() });
            }

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            ViewBag.AlbumId = albumId;
            ViewBag.IsSearchResult = false;
            list.Reverse();

            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult AddImg(int albumId)
        {
            ViewBag.AlbumId = albumId;
            return PartialView("AddImg");
        }

        [HttpPost]
        public ActionResult AddImg(HttpPostedFileBase [] files, int albumId, string tags)
        {
            var user = userManager.GetUsers().Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            byte[] array = null;

            if (files.Length > 0)
            foreach (var file in files)
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Media"), pic);
                var tagsDto = new List<TagDTO>();

                file.SaveAs(path);

                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    array = ms.GetBuffer();
                }

                foreach (var el in tags.Split(' '))
                    tagsDto.Add(new TagDTO() { Name = el });

                mediaService.AddImage(new PictureDTO(){Img = array, Tags = tagsDto }, albumId);
            }

            return Redirect($"/Media/Images?albumId={albumId}");
        }

        #endregion

        #region Albums
        public ActionResult Albums(int? page)
        {
            var user = userManager.GetUsers().Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            var albums = mediaService.GetAlbumsForUser(user.Id);
            var list = new List<AlbumModel>();
            byte[] defaultImg = System.IO.File.ReadAllBytes(AppContext.BaseDirectory + "Media/album-img.png");

            foreach (var album in albums)
                list.Add(new AlbumModel()
                {
                    Id = album.Id,
                    Name = album.Name,
                    Description = album.Description,
                    Img = album.Pictures.Count > 0 ? album.Pictures.ElementAt(new Random().Next(0, album.Pictures.Count)).Img : defaultImg
                });

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            list.Reverse();

            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet, ActionName("Create")]
        public ActionResult CreateAlbum()
        {
            return PartialView("CreateAlbum");
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreateAlbum(AlbumModel model)
        {
            var user = userManager.GetUsers().Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            var album = new AlbumDTO() { Name = model.Name, Description = model.Description };
            mediaService.AddAlbum(album, user.Id);

            return RedirectToAction("Albums");
        }

        [HttpGet, ActionName("Edit")]
        public ActionResult EditAlbum(int id)
        {
            var album = mediaService.GetAlbumById(id);

            var model = new AlbumModel() { Id = album.Id, Name = album.Name, Description = album.Description };
            return PartialView("EditAlbum", model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditAlbumConfirmation(AlbumModel model)
        {
            var albumDto = new AlbumDTO()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };

            mediaService.EditAlbum(albumDto);

            return RedirectToAction("Albums");
        }

        [HttpGet, ActionName("Remove")]
        public ActionResult RemoveAlbum(int id)
        {
            var album = mediaService.GetAlbumById(id);
            var model = new AlbumModel() { Id = album.Id, Name = album.Name, Description = album.Description };
            return PartialView("RemoveAlbum", model);
        }

        [HttpPost, ActionName("Remove")]
        public ActionResult RemoveAlbumConfirmation(int id)
        {
            mediaService.RemoveAlbum(id);
            return RedirectToAction("Albums");
        }
    }
    #endregion
}