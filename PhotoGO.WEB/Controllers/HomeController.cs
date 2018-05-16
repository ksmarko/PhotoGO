using System.Web.Mvc;

namespace PhotoGO.WEB.Controllers
{
    public class HomeController : Controller
    {
        #region Main
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Albums");

            return View();
        }
        #endregion
    }
}