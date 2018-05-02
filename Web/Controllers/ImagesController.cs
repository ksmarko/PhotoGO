using BLL.DTO;
using BLL.Interfaces;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class ImagesController : Controller
    {
        readonly IImageService imageService;
        readonly IUserManager userManager;

        public ImagesController(IImageService imageService, IUserManager userManager)
        {
            this.imageService = imageService;
            this.userManager = userManager;
        }

        public ActionResult Search(string tag, int? page)
        {
            var user = userManager.GetUsers().Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            var images = imageService.SearchImages(tag);
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            ViewBag.IsSearchResult = true;
            ViewBag.Tag = tag;
            var list = new List<ImageModel>();

            foreach (var img in images)
            {
                list.Add(new ImageModel()
                {
                    Id = img.Id,
                    Img = img.Img,
                    Likes = img.FavouritedBy.Count,
                    IsLiked = imageService.IsLikedBy(user.Id, img.Id),
                    Tags = img.Tags.Select(x => x.Name).ToList()
                });
            }

            return View("Index", list.ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        public ActionResult Index(int albumId, int? page)
        {
            var user = userManager.GetUsers().Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            if (!user.Albums.Any(x => x.Id == albumId))
                return new HttpNotFoundResult();

            var imgs = imageService.GetImages(albumId).ToList();
            var list = new List<ImageModel>();

            foreach (var img in imgs)
            {
                list.Add(new ImageModel()
                {
                    Id = img.Id,
                    Img = img.Img,
                    Likes = img.FavouritedBy.Count,
                    IsLiked = imageService.IsLikedBy(user.Id, img.Id),
                    Tags = img.Tags.Select(x => x.Name).ToList()
                });
            }

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            ViewBag.AlbumId = albumId;
            ViewBag.IsSearchResult = false;
            list.Reverse();

            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddImage(int albumId)
        {
            ViewBag.AlbumId = albumId;
            return PartialView();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddImage(HttpPostedFileBase[] files, int albumId, string tags)
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

                        imageService.AddImage(new PictureDTO() { Img = array, Tags = tagsDto }, albumId);
                    }

            return Redirect($"/Images/Index?albumId={albumId}");
        }

        [HttpGet]
        [Authorize]
        public ActionResult RemoveImage(int id)
        {
            var el = imageService.GetImageById(id);
            var img = new ImageModel() { Id = el.Id, Img = el.Img, Likes = el.FavouritedBy.Count, Tags = el.Tags.Select(x => x.Name).ToList() };

            return PartialView(img);
        }

        [Authorize]
        [HttpPost, ActionName("RemoveImage")]
        public ActionResult RemoveImageConfirmed(int id)
        {
            var albumId = imageService.GetImageById(id).Album.Id;
            imageService.RemoveImage(id);
            return Redirect($"/Images/Index?albumId={albumId}");
        }
    }
}