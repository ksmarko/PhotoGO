using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using AutoMapper;
using PhotoGO.BLL.DTO;
using PhotoGO.WEB.Models;
using PhotoGO.WEB.Helpers;
using PhotoGO.BLL.Interfaces;

namespace PhotoGO.WEB.Controllers
{
    [Authorize]
    public class AlbumsController : Controller
    {
        #region Fields
        readonly IAlbumService albumService;
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
        public AlbumsController (IAlbumService albumService, IUserManager userManager)
        {
            this.albumService = albumService;
            this.userManager = userManager;
        }
        #endregion

        #region Main
        public ActionResult Index(int? page)
        {
            var albums = CurrentUser.Albums;
            byte[] defaultImg = System.IO.File.ReadAllBytes(AppContext.BaseDirectory + "favicon.ico");

            var list = Mapper.Map<IEnumerable<AlbumDTO>, ICollection<AlbumModel>>(albums.Reverse());
            list.Where(x => x.Img.Length <= 0).ToList().ForEach(x => x.Img = defaultImg);

            return View(CreatePagedList.From(list, page));
        }
        #endregion

        #region CreateAlbum
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlbumModel model)
        {
            if (ModelState.IsValid)
            {
                var album = Mapper.Map<AlbumModel, AlbumDTO>(model);
                album.UserId = CurrentUser.Id;

                albumService.AddAlbum(album);

                return RedirectToAction("Index");
            }
            return PartialView();
        }
        #endregion

        #region EditAlbum
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!IsUserAlbum(id))
                return new HttpNotFoundResult();

            var album = albumService.GetAlbumById(id);
            var model = Mapper.Map<AlbumDTO, AlbumModel>(album);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(AlbumModel model)
        {
            if (ModelState.IsValid)
            {
                var album = Mapper.Map<AlbumModel, AlbumDTO>(model);
                albumService.EditAlbum(album);

                return RedirectToAction("Index");
            }
            return PartialView(model);
        }
        #endregion

        #region RemoveAlbum
        [HttpGet, ActionName("Remove")]
        public ActionResult RemoveAlbum(int id)
        {
            if (!IsUserAlbum(id))
                return new HttpNotFoundResult();

            var album = albumService.GetAlbumById(id);
            var model = Mapper.Map<AlbumDTO, AlbumModel>(album);

            return PartialView(model);
        } 

        [HttpPost, ActionName("Remove")]
        public ActionResult RemoveAlbumConfirmation(int id)
        {
            albumService.RemoveAlbum(id);
            return RedirectToAction("Index");
        }
        #endregion

        #region Private
        private bool IsUserAlbum(int albumId)
        {
            if (CurrentUser.Albums.Any(x => x.Id == albumId))
                return true;

            return false;
        }
        #endregion
    }
}