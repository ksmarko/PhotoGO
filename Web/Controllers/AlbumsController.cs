using AutoMapper;
using PhotoGO.BLL.DTO;
using PhotoGO.BLL.Interfaces;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class AlbumsController : Controller
    {
        readonly IAlbumService albumService;
        readonly IUserManager userManager;

        private UserDTO user;

        UserDTO CurrentUser
        {
            get
            {
                if (user == null)
                    user = userManager.GetUserByName(User.Identity.Name);
                return user;
            }
        }

        public AlbumsController (IAlbumService albumService, IUserManager userManager)
        {
            this.albumService = albumService;
            this.userManager = userManager;
        }

        public ActionResult Index(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var albums = CurrentUser.Albums;
            byte[] defaultImg = System.IO.File.ReadAllBytes(AppContext.BaseDirectory + "favicon.ico");

            var list = Mapper.Map<IEnumerable<AlbumDTO>, ICollection<AlbumModel>>(albums.Reverse());
            list.Where(x => x.Img.Length <= 0).ToList().ForEach(x => x.Img = defaultImg);

            return View(list.ToPagedList(pageNumber, pageSize));
        }

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

        private bool IsUserAlbum(int albumId)
        {
            if (CurrentUser.Albums.Any(x => x.Id == albumId))
                return true;

            return false;
        }
    }
}