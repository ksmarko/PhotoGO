using AutoMapper;
using PhotoGO.BLL.DTO;
using PhotoGO.BLL.Interfaces;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PhotoGO.WEB.Models;

namespace PhotoGO.WEB.Controllers
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
        public async Task<ActionResult> EditRoles(string id, string role)
        {
            if (ModelState.IsValid)
            {
                await userManager.EditRole(id, role);
                return RedirectToAction("Users");
            }
            return View();
        }

        [Authorize(Roles = "admin, moderator")]
        public ActionResult AddTags(int imgId)
        {
            var tags = imageService.GetImageById(imgId).Tags;
            string res = string.Join(" ", tags.Select(x => x.Name));

            ViewBag.ImgId = imgId;
            var model = new TagsModel() { Tags = res };

            return PartialView(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin, moderator")]
        public ActionResult AddTags(TagsModel model, int imgId)
        {
            imageService.AddTags(imgId, Regex.Replace(model.Tags, @"\s+", " ").Trim().Split(' ').ToArray());
            return Redirect("/Images/Manage");
        }
    }
}