using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class ManagementController : Controller
    {
        readonly IUserManager userManager;
        readonly IMediaService mediaService;

        public ManagementController(IUserManager userManager, IMediaService mediaService)
        {
            this.userManager = userManager;
            this.mediaService = mediaService;
        }

        [Authorize(Roles = "admin")]
        public ActionResult Users()
        {
            ViewBag.Roles = new SelectList(userManager.GetRoles());
            var users = Mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserModel>>(userManager.GetUsers());

            return View("UserManagement", users);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public virtual ActionResult EditRoles(string id, string role)
        {
            if (ModelState.IsValid)
            {
                var oldUser = userManager.GetUserById(id);
                var oldRoleName = oldUser.Role;

                if (oldRoleName != role)
                {
                    userManager.RemoveFromRole(id, oldRoleName);
                    userManager.AddToRole(id, role);
                }
                return RedirectToAction("Users");
            }
            return View();
        }

        [Authorize(Roles = "admin, moderator")]
        public ActionResult Media(int? page)
        {
            var images = mediaService.GetImages();
            var list = new List<ImageModel>();

            foreach (var img in images)
                list.Add(new ImageModel() {Id = img.Id, Img = img.Img, Likes = img.FavouritedBy.Count, Tags = img.Tags.Select(x => x.Name).ToList() });

            int pageSize = 12;
            int pageNumber = (page ?? 1);
            ViewBag.Page = page;

            list.Reverse();

            return View("MediaManagement", list.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "admin, moderator")]
        public ActionResult AddTags(int imgId)
        {
            var tags = mediaService.GetImageById(imgId).Tags;
            string res = "";
            foreach (var el in tags)
                res += el.Name + " ";

            ViewBag.ImgId = imgId;
            return PartialView("AddTags", res);
        }

        [HttpPost]
        [Authorize(Roles = "admin, moderator")]
        public ActionResult AddTags(string tags, int imgId)
        {
            mediaService.AddTags(imgId, tags.Split(' ').ToArray());

            return RedirectToAction("Media");
        }

        [HttpGet]
        [Authorize(Roles = "admin, moderator")]
        public ActionResult Remove(int id)
        {
            var el = mediaService.GetImageById(id);
            var img = new ImageModel() { Id = el.Id, Img = el.Img, Likes = el.FavouritedBy.Count, Tags = el.Tags.Select(x => x.Name).ToList() };

            return PartialView(img);
        }

        [HttpPost, ActionName("Remove")]
        [Authorize(Roles = "admin, moderator")] 
        public ActionResult RemoveConfirmed(int id)
        {
            mediaService.RemoveImage(id);
            return RedirectToAction("Media");
        }
    }
}