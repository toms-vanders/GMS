using GMS___Model;
using GMS___Web_Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace GMS___Web_Client.Controllers
{
    public class AuthController : Controller
    {
        public static string EndPoint = "https://localhost:44377/";


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
                try
                {
                    StartSession(new User(model.Username, model.Password));
                    return RedirectToAction("UserPage", "User");
                } catch
                {
                    ViewBag.Error = "Invalid information was given.";
                    return View();
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
            this.Session["UserToken"] = PostLogin(user);
            User tempUser = GetJson<User>("api/user");
            this.Session["ApiToken"] = tempUser.ApiKey;
            this.Session["EmailAddress"] = tempUser.EmailAddress;
            this.Session["Username"] = tempUser.UserName;
        }

        public string PostLogin(User user)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.BaseAddress = EndPoint;
                    webClient.Encoding = System.Text.Encoding.UTF8;
                    webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                    return webClient.UploadString("api/user/login", JsonConvert.SerializeObject(user));
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        //T is a generic type, need to test this more
        public T GetJson<T>(string urlSuffix)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.BaseAddress = EndPoint;
                    if (urlSuffix.Contains("gw2"))
                    {
                        webClient.Headers.Add("Authorization", this.Session["ApiToken"].ToString());
                    } else
                    {
                        webClient.Headers.Add("Authorization", this.Session["UserToken"].ToString());
                    }
                    webClient.Encoding = System.Text.Encoding.UTF8;
                    return JsonConvert.DeserializeObject<T>(webClient.DownloadString(urlSuffix)); ;
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
                    if (!(this.Session["UserToken"] is null))
                        webClient.Headers.Add("Authorization", this.Session["UserToken"].ToString());
                    return JsonConvert.DeserializeObject<T>(webClient.UploadString(urlSuffix, JsonConvert.SerializeObject(postObject)));
                }
            } catch (WebException ex)
            {
                throw ex;
            }
        }
    }
}