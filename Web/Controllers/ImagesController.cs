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
            var images = imageService.SearchImages(tag);
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            ViewBag.IsSearchResult = true;
            ViewBag.Tag = tag;

            return View("Index", FillImagesList(images).ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        public ActionResult Favourites(int? page)
        {
            var images = imageService.GetFavouritesForUser(GetUser().Id);
            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(FillImagesList(images).ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        public ActionResult Index(int albumId, int? page)
        {
            if (!IsUserAlbum(albumId))
                return new HttpNotFoundResult();

            var images = imageService.GetImages(albumId).ToList();
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            ViewBag.AlbumId = albumId;
            ViewBag.IsSearchResult = false;
            ViewBag.Description = GetUser().Albums.Where(x => x.Id == albumId).FirstOrDefault().Description;

            return View(FillImagesList(images).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddImage(int albumId)
        {
            if (!IsUserAlbum(albumId))
                return new HttpNotFoundResult();

            ViewBag.AlbumId = albumId;
            return PartialView();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddImage(AddImageModel model, int albumId)
        {
            byte[] array = null;

            if (model.files.Length > 0)
                foreach (var file in model.files)
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

                        foreach (var el in model.tags.Split(' '))
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

            if (!IsUserImage(id))
                return new HttpNotFoundResult();

            var img = new ImageModel() { Id = el.Id, Img = el.Img };

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

        private List<ImageModel> FillImagesList(IEnumerable<PictureDTO> pictures)
        {
            var list = new List<ImageModel>();

            foreach (var img in pictures)
            {
                list.Add(new ImageModel()
                {
                    Id = img.Id,
                    Img = img.Img,
                    Likes = img.FavouritedBy.Count,
                    IsLiked = imageService.IsLikedBy(GetUser().Id, img.Id),
                    IsMy = IsUserImage(img.Id) ? true : false,
                    Tags = img.Tags.Select(x => x.Name).ToList()
                });
            }

            return list;
        }

        public int Like(int id)
        {
            //BUG: if one user delete his image and another user like it 
            //TODO: refresh data for all users (???)

            imageService.LikeImage(id, GetUser().Id);

            return imageService.GetImageById(id).FavouritedBy.Count;
        }

        private bool IsUserImage(int imgId)
        {
            var el = imageService.GetImageById(imgId);

            if (GetUser().Albums.Any(x => x.Id == el.Album.Id))
                return true;

            return false;
        }

        private bool IsUserAlbum(int albumId)
        {
            if (GetUser().Albums.Any(x => x.Id == albumId))
                return true;

            return false;
        }

        private UserDTO GetUser()
        {
            return userManager.GetUsers().Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
        }
    }
}