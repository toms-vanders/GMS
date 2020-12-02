using GMS___Model;
using GMS___Web_Client.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;

namespace GMS___Web_Client.Controllers
{
    public class AuthController : Controller
    {
        public static string EndPoint = "https://localhost:44377/";
        public static string ApiKey;


        // GET: Auth
        public ActionResult Index()
        {
            if (InSession())
            {
                return RedirectToAction("UserPage", "User");
            }
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
            EndSession();
            return RedirectToAction("LogIn", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(UserModel model)
        {
            if (ModelState.IsValid)
            {
                if (PostJson("api/user/signup", new User(model.UserName, model.EmailAddress, model.Password)) != null)
                {
                    return RedirectToAction("Index", "Home");
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
                User user = PostJson("api/user/login", new User(model.EmailAddress, model.Password));
                if (user != null)
                {
                    StartSession(user);
                    return RedirectToAction("UserPage", "User");
                }
            }
            ViewBag.Error = "Invalid information was given.";
            return View();
        }

        protected Boolean InSession()
        {
            return this.Session["Username"] != null;
        }

        protected void EndSession()
        {
            FormsAuthentication.SignOut();
            this.Session.Abandon();
        }

        protected void StartSession(User user)
        {
            if (user.ApiKey == "")
            {
                //TODO: retrieve api Key page
            }
            ApiKey = user.ApiKey;
            this.Session["ApiToken"] = ApiKey;
            this.Session["EmailAddress"] = user.EmailAddress;
            this.Session["Username"] = user.UserName;
        }

        //T is a generic type, need to test this more
        public T GetJson<T>(string urlSuffix, T returnType)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.BaseAddress = EndPoint;
                    webClient.Headers.Add("Authorization", this.Session["ApiToken"].ToString());
                    webClient.Encoding = System.Text.Encoding.UTF8;
                    var json = webClient.DownloadString(urlSuffix);
                    T t = JsonConvert.DeserializeObject<T>(json);
                    return t;
                }
            } catch (WebException ex)
            {
                throw ex;
            }
        }

        public T PostJson<T>(string urlSuffix, T postObject)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.BaseAddress = EndPoint;
                    webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string data = JsonConvert.SerializeObject(postObject);
                    var response = webClient.UploadString(urlSuffix, data);
                    return JsonConvert.DeserializeObject<T>(response);
                }
            } catch (WebException ex)
            {
                throw ex;
            }
        }
    }
}