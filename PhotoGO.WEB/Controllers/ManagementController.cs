using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AutoMapper;
using PhotoGO.BLL.DTO;
using PhotoGO.WEB.Models;
using PhotoGO.BLL.Interfaces;

namespace PhotoGO.WEB.Controllers
{
    public class ManagementController : Controller
    {
        #region Fields
        readonly IUserManager userManager;
        readonly IImageService imageService;
        #endregion

        #region Ctor
        public ManagementController(IUserManager userManager, IImageService imageService)
        {
            this.userManager = userManager;
            this.imageService = imageService;
        }
        #endregion

        #region UserManagement
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
                await userManager.EditRole(id, role);

            return RedirectToAction("Users");
        }
        #endregion

        #region AddTags
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
            if (ModelState.IsValid)
                imageService.AddTags(imgId, Regex.Replace(model.Tags, @"\s+", " ").Trim().Split(' ').ToArray());

            return Redirect("/Images/Manage");
        }
        #endregion
    }
}