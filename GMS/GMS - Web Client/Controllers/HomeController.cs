using GMS___Business_Layer;
using GMS___Web_Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GMS___Web_Client.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult SignUp()
        {
            ViewBag.Message = "Your sign-up page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(UserModel model)
        {
            if (ModelState.IsValid)
            {
                UserProcessor userProcessor = new UserProcessor();
                bool isCreated = userProcessor.InsertNewUser(model.UserName,
                    model.EmailAddress,model.Password);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}