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
        readonly IImageService imageService;

        public ManagementController(IUserManager userManager, IImageService imageService)
        {
            this.userManager = userManager;
            this.imageService = imageService;
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
        public ActionResult AddTags(int imgId)
        {
            var tags = imageService.GetImageById(imgId).Tags;
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
            imageService.AddTags(imgId, tags.Split(' ').ToArray());

            return Redirect("/Images/Manage");
        }
    }
}