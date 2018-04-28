using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
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

        public ManagementController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public ActionResult Manage()
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
                return RedirectToAction("Manage");
            }
            return View();
        }
    }
}