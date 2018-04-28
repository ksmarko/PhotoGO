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
    public class ProfileController : Controller
    {
        readonly IMediaService mediaService;

        public ProfileController (IMediaService ms)
        {
            mediaService = ms;
        }

        public ActionResult Index()
        {
            return View("UserProfile");
        }

        public ActionResult Albums()
        {
            var albums = new List<AlbumModel>()
            {
                new AlbumModel() {Name = "Name1", Description="Description1"},
                new AlbumModel() {Name = "Name2", Description="Description2"},
                new AlbumModel() {Name = "Name3", Description="Description3"},
                new AlbumModel() {Name = "Name4", Description="Description4"},
                new AlbumModel() {Name = "Name5", Description="Description5"}
            };

            return PartialView("Albums", albums);
        }

        public ActionResult Favourites()
        {
            return Content("likes are here");
        }

        public ActionResult Settings()
        {
            return Content("this is settings");
        }
    }
}