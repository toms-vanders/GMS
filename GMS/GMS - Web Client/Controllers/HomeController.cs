using GMS___Business_Layer;
using GMS___Model;
using GMS___Web_Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GMS___Web_Client.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(this.Session["Username"] != null)
            {
                return RedirectToAction("UserPage");
            }
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
        public ActionResult LogIn()
        {
            ViewBag.Message = "Your login page.";

            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            this.Session.Abandon();
            return RedirectToAction("LogIn");
        }
        public ActionResult UserPage()
        {
            if (this.Session["Username"] != null)
            {
                ViewBag.Message = "Your user page.";
                return View();
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index");
        }
        public ActionResult ApiForm()
        {
            if (this.Session["Username"] != null)
            {
                ViewBag.Message = "Your API form.";
                return View();
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(UserModel model)
        {
            if (ModelState.IsValid)
            {
                UserProcessor userProcessor = new UserProcessor();
                if (userProcessor.InsertNewUser(model.UserName, model.EmailAddress, model.Password))
                {
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Error = "Invalid information was given.";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LogInModel model)
        {
            if (ModelState.IsValid)
            {
                UserProcessor userProcessor = new UserProcessor();
                User user = userProcessor.LogInUser(model.EmailAddress, model.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.EmailAddress, false);
                    this.Session["EmailAddress"] = user.EmailAddress;
                    this.Session["Username"] = user.UserName;
                    return RedirectToAction("UserPage");
                }
            }
            ViewBag.Error = "Invalid information was given.";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApiForm(ApiModel model)
        {
            if (ModelState.IsValid)
            {
                UserProcessor userProcessor = new UserProcessor();
                if (userProcessor.InsertApiKey(this.Session["EmailAddress"].ToString(), model.ApiKey))
                {
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Error = "Invalid information was given.";
            return View();
        }
    }
}