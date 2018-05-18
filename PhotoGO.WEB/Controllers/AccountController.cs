using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using PhotoGO.BLL.DTO;
using PhotoGO.WEB.Models;
using PhotoGO.BLL.Interfaces;
using PhotoGO.BLL.Infrastructure;

namespace PhotoGO.WEB.Controllers
{
    public class AccountController : Controller
    {
        #region Fields
        readonly IUserManager userManager;

        private IAuthenticationManager AuthenticationManager
        {
            get => HttpContext.GetOwinContext().Authentication;
        }
        #endregion

        #region Ctor
        public AccountController(IUserManager userManager) => this.userManager = userManager;
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Login
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
        #endregion
        
        #region Register
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
        #endregion
    }
}