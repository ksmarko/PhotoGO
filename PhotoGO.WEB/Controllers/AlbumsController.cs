using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using AutoMapper;
using PhotoGO.BLL.DTO;
using PhotoGO.WEB.Models;
using PhotoGO.WEB.Helpers;
using PhotoGO.BLL.Interfaces;
using PhotoGO.BLL.Exceptions;
using System.IO;

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
            var list = Mapper.Map<IEnumerable<AlbumDTO>, ICollection<AlbumModel>>(albums.Reverse());
            FileHelper.SetDefaultImage(ref list);

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
                try
                {
                    var album = Mapper.Map<AlbumModel, AlbumDTO>(model);
                    album.UserId = CurrentUser.Id;

                    albumService.AddAlbum(album);

                    return RedirectToAction("Index");
                }
                catch (ArgumentNullException e)
                {
                    ModelState.AddModelError("Error", e.Message);
                }
                catch (UserNotFoundException e)
                {
                    ModelState.AddModelError("Error", e.Message);
                }
            }
            return PartialView(model);
        }
        #endregion

        #region EditAlbum
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!IsUserAlbum(id))
                return new HttpNotFoundResult();

            try
            {
                var album = albumService.GetAlbumById(id);
                var model = Mapper.Map<AlbumDTO, AlbumModel>(album);

                return PartialView(model);
            }
            catch (TargetNotFoundException e)
            {
                ModelState.AddModelError("Error", e.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(AlbumModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var album = Mapper.Map<AlbumModel, AlbumDTO>(model);
                    albumService.EditAlbum(album);

                    return RedirectToAction("Index");
                }
                catch (ArgumentNullException e)
                {
                    ModelState.AddModelError("Error", e.Message);
                }
                catch (TargetNotFoundException e)
                {
                    ModelState.AddModelError("Error", e.Message);
                }

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

            try
            {
                var album = albumService.GetAlbumById(id);
                var model = Mapper.Map<AlbumDTO, AlbumModel>(album);

                return PartialView(model);
            }
            catch (TargetNotFoundException e)
            {
                ModelState.AddModelError("Error", e.Message);
            }

            return PartialView(id);
        } 

        [HttpPost, ActionName("Remove")]
        public ActionResult RemoveAlbumConfirmation(int id)
        {
            try
            {
                albumService.RemoveAlbum(id);
            }
            catch (TargetNotFoundException e)
            {
                ModelState.AddModelError("Error", e.Message);
                return PartialView(id);
            }
            
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