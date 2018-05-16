﻿using PhotoGO.BLL.DTO;
using PhotoGO.BLL.Infrastructure;
using PhotoGO.BLL.Interfaces;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PhotoGO.WEB.Models;

namespace PhotoGO.WEB.Controllers
{
    public class AccountController : Controller
    {
        readonly IUserManager userManager;

        private IAuthenticationManager AuthenticationManager
        {
            get => HttpContext.GetOwinContext().Authentication;
        }

        public AccountController(IUserManager userManager) => this.userManager = userManager;

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await userManager.Authenticate(userDto);

                if (claim == null)
                    ModelState.AddModelError("", "Invalid login or password");
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claim);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    Role = "user"
                };
                OperationDetails operationDetails = await userManager.Create(userDto);
                if (operationDetails.Succedeed)
                {
                    ClaimsIdentity claim = await userManager.Authenticate(userDto);
                    AuthenticationManager.SignIn(new AuthenticationProperties{ IsPersistent = true }, claim);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }
    }
}