using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PhotoGO.BLL.DTO;
using PhotoGO.WEB.Models;
using PhotoGO.WEB.Helpers;
using PhotoGO.BLL.Interfaces;
using AutoMapper;
using Web.Helpers;

namespace PhotoGO.WEB.Controllers
{
    public class ImagesController : Controller
    {
        #region Fields
        readonly IImageService imageService;
        readonly IUserManager userManager;

        UserDTO user;

        UserDTO CurrentUser
        {
            get
            {
                if (user == null)
                    user = userManager.GetUserByName(User.Identity.Name);
                return user;
            }
        }
        #endregion

        #region Ctor
        public ImagesController(IImageService imageService, IUserManager userManager)
        {
            this.imageService = imageService;
            this.userManager = userManager;
        }
        #endregion

        #region Search
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
            if (tags == null)
                return View("Index", CreatePagedList.Empty<ImageModel>());

            tags = Regex.Replace(tags, @"\s+", " ").Trim();
            var images = imageService.SearchImages(tags.Split(' '));

            ViewBag.Tag = tags;
            SetPageRole(PageRole.Search);

            if (images == null)
                return View("Index", CreatePagedList.Empty<ImageModel>());

            return View("Index", CreatePagedList.From(FillImagesList(images), page));
        }
        #endregion

        #region Pages
        [Authorize]
        public ActionResult Favourites(int? page)
        {
            var images = CurrentUser.LikedPictures;
            images.Reverse();
            SetPageRole(PageRole.Favourites);

            return View("Index", CreatePagedList.From(FillImagesList(images), page));
        }

        [Authorize(Roles = "admin, moderator")]
        public ActionResult Manage(int? page)
        {
            var images = imageService.GetImages();
            images.Reverse();
            SetPageRole(PageRole.Management);

            return View("Index", CreatePagedList.From(FillImagesList(images), page));
        }

        [Authorize]
        public ActionResult Index(int albumId, int? page)
        {
            if (!IsUserAlbum(albumId))
                return new HttpNotFoundResult();

            var images = imageService.GetImages(albumId).ToList();

            ViewBag.AlbumId = albumId;
            SetPageRole(PageRole.Index);
            ViewBag.Description = CurrentUser.Albums.Where(x => x.Id == albumId).FirstOrDefault().Description;

            images.Reverse();

            return View(CreatePagedList.From(FillImagesList(images), page));
        }
        #endregion

        #region AddImage
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
        #endregion

        #region RemoveImage
        [HttpGet]
        [Authorize]
        public ActionResult RemoveImage(int id)
        {
            var el = imageService.GetImageById(id);

            if (!IsUserImage(id))
                return new HttpNotFoundResult();

            var img = Mapper.Map<PictureDTO, ImageModel>(el);

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
        #endregion

        #region LikeImage
        [Authorize]
        public int Like(int id)
        {
            imageService.LikeImage(id, CurrentUser.Id);
            var img = imageService.GetImageById(id);

            return img == null? 0 : img.FavouritedBy.Count;
        }
        #endregion

        #region Private
        private List<ImageModel> FillImagesList(IEnumerable<PictureDTO> pictures)
        {
            var list = Mapper.Map<IEnumerable<PictureDTO>, IEnumerable<ImageModel>>(pictures).ToList();
            list.ForEach(x => x.IsLiked = User.Identity.IsAuthenticated ? imageService.IsLikedBy(CurrentUser.Id, x.Id) : false);
            list.ForEach(x => x.IsMy = User.Identity.IsAuthenticated ? (IsUserImage(x.Id) ? true : false) : false);

            return list;
        }
        
        private bool IsUserImage(int imgId)
        {
            var el = imageService.GetImageById(imgId);

            if (CurrentUser.Albums.Any(x => x.Id == el.AlbumId))
                return true;

            return false;
        }

        private bool IsUserAlbum(int albumId)
        {
            if (CurrentUser.Albums.Any(x => x.Id == albumId))
                return true;

            return false;
        }

        private void SetPageRole(PageRole role)
        {
            ViewData["PageRole"] = role.ToString();
        }
        #endregion
    }
}