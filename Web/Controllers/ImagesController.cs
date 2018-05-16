using BLL.DTO;
using BLL.Interfaces;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public ActionResult Autocomplete(string term)
        {
            var items = imageService.GetTags();
            term = term.Substring(term.LastIndexOf(' ') + 1);
            List<string> filteredItems = items.Where(item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0 ).ToList();
            filteredItems.Sort();

            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult Search(int? page, string tags)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            ViewBag.IsSearchResult = true;
            ViewBag.IsManagement = false;
            ViewBag.IsIndex = false;
            ViewBag.IsFavourites = false;

            if (tags == null)
                return View("Index", new List<ImageModel> { }.ToPagedList(pageNumber, pageSize));

            tags = System.Text.RegularExpressions.Regex.Replace(tags, @"\s+", " ").Trim();
            var images = imageService.SearchImages(tags.Split(' '));
            ViewBag.Tag = tags;

            if (images == null)
                return View("Index", new List<ImageModel> { }.ToPagedList(pageNumber, pageSize));

            return View("Index", FillImagesList(images).ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        public ActionResult Favourites(int? page)
        {
            var images = GetUser().LikedPictures;
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            ViewBag.IsSearchResult = false;
            ViewBag.IsManagement = false;
            ViewBag.IsIndex = false;
            ViewBag.IsFavourites = true;

            images.Reverse();

            return View("Index", FillImagesList(images).ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "admin, moderator")]
        public ActionResult Manage(int? page)
        {
            var images = imageService.GetImages();
            images.Reverse();
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            ViewBag.Page = page;
            ViewBag.IsSearchResult = false;
            ViewBag.IsManagement = true;
            ViewBag.IsIndex = false;
            ViewBag.IsFavourites = false;

            return View("Index", FillImagesList(images).ToPagedList(pageNumber, pageSize));
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
            ViewBag.IsManagement = false;
            ViewBag.IsIndex = true;
            ViewBag.IsFavourites = false;
            ViewBag.Description = GetUser().Albums.Where(x => x.Id == albumId).FirstOrDefault().Description;

            images.Reverse();

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

            if (model.Files.Length > 0)
                foreach (var file in model.Files)
                    if (file != null)
                    {
                        var tagsDto = new List<TagDTO>();

                        using (MemoryStream ms = new MemoryStream())
                        {
                            file.InputStream.CopyTo(ms);
                            array = ms.GetBuffer();
                        }

                        if (!string.IsNullOrWhiteSpace(model.Tags))
                            foreach (var el in model.Tags.Split(' '))
                                tagsDto.Add(new TagDTO() { Name = el.Trim() });

                        imageService.AddImage(new PictureDTO() { Img = array, Tags = tagsDto, AlbumId = albumId });
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
            var albumId = imageService.GetImageById(id).AlbumId;
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
                    IsLiked = User.Identity.IsAuthenticated? imageService.IsLikedBy(GetUser().Id, img.Id) : false,
                    IsMy = User.Identity.IsAuthenticated ? (IsUserImage(img.Id) ? true : false) : false,
                    Tags = img.Tags.Select(x => x.Name).ToList()
                });
            }

            return list;
        }

        [Authorize]
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

            if (GetUser().Albums.Any(x => x.Id == el.AlbumId))
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