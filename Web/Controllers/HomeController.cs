using BLL.Interfaces;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        readonly IMediaService mediaService;
        readonly IUserManager userManager;

        public HomeController(IMediaService mediaService, IUserManager userManager)
        {
            this.mediaService = mediaService;
            this.userManager = userManager;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}