using System.Web.Mvc;

namespace GMS___Web_Client.Controllers
{
    public class HomeController : AuthController
    {
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}