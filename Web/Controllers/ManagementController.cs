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
    [Authorize(Roles = "admin")]
    public class ManagementController : Controller
    {
        readonly IUserManager userManager;
        readonly IMediaService mediaService;

        public ManagementController(IUserManager userManager, IMediaService mediaService)
        {
            this.userManager = userManager;
            this.mediaService = mediaService;
        }

        public ActionResult Users()
        {
            ViewBag.Roles = new SelectList(userManager.GetRoles());
            var users = Mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserModel>>(userManager.GetUsers());

            return View("UserManagement", users);
        }

        [HttpPost]
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

        public ActionResult Media(int? page)
        {
            var images = mediaService.GetImages();
            var list = new List<ImageModel>();

            foreach (var img in images)
                list.Add(new ImageModel() {Id = img.Id, Img = img.Img, Likes = img.FavouritedBy.Count, Tags = img.Tags });

            int pageSize = 12;
            int pageNumber = (page ?? 1);
            ViewBag.Page = page;

            list.Reverse();

            return View("MediaManagement", list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddTag(int imgId)
        {
            return null;
        }

        public ActionResult Remove(int imgId, int? page)
        {
            mediaService.RemoveImage(imgId);
            return RedirectToAction("Media", page);
        }
    }
}