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
    public class ManagementController : Controller
    {
        readonly IUserManager userManager;

        public ManagementController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public ActionResult Manage()
        {
            var users = Mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserModel>>(userManager.GetUsers());
            return View("UserManagement", users);
        }
    }
}