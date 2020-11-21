using GMS___Business_Layer;
using GMS___Model;
using GMS___Web_Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GMS___Web_Client.Controllers
{
    public class HomeController : Controller
    {
        public static string EndPoint = "https://localhost:44377/";

        public ActionResult Index()
        {
            if(InSession())
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
            EndSession();
            return RedirectToAction("LogIn");
        }
        public ActionResult UserPage()
        {
            if (InSession())
            {
                this.Session["Guild"] = "";
                ArrayList characterList = new ArrayList();
                ArrayList characterNameList = GetJson<ArrayList>("gw2api/characters", new ArrayList());
                foreach(string name in characterNameList)
                {
                    string urlSuffix = "gw2api/characters/" + name + "/core";
                    characterList.Add(GetJson<Character>(urlSuffix, new Character()));
                }
                ViewBag.Characters = characterList;
                ViewBag.Message = "Your user page.";
                return View();
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index");
        }
        public ActionResult CharacterPage(string name)
        {
            if (InSession())
            {
                string urlSuffix = "gw2api/characters/" + name + "/core";
                ViewBag.Character = GetJson<Character>(urlSuffix, new Character());
                ViewBag.Message = "Your character page.";
                this.Session["Guild"] = ViewBag.Character.Guild;
                return View();
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index");
        }
        public ActionResult ApiForm()
        {
            if (InSession())
            {
                ViewBag.Message = "Your API form.";
                return View();
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index");
        }

        public ActionResult CreateEventForm()
        {
            if (InSession())
            {
                // Getting all event types needed for DropDownList
                var eventTypes = GetAllEventTypes();
                var model = new EventModel();
                // Get list of SelectListItem(s)
                model.EventTypes = GetOptionEventTypesList(eventTypes);
                ViewBag.Message = "Creating event.";
                return View(model);
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index");
        }

        public ActionResult SearchEvents()
        {
            if (this.Session["Username"] != null)
            {
                // Getting all event types needed for DropDownList
                var eventTypes = GetAllEventTypes();
                var model = new EventModel();
                // Get list of SelectListItem(s)
                model.EventTypes = GetOptionEventTypesList(eventTypes);
                ViewBag.Message = "Find new events";
                return View(model);
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
                if (PostJson(new User(model.UserName, model.EmailAddress,model.Password), "api/user/signup") !=null)
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
                User user = PostJson(new User(model.EmailAddress, model.Password), "api/user/login");
                if (user != null)
                {
                    StartSession(user);
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
                User user = new User();
                user.EmailAddress = this.Session["EmailAddress"].ToString();
                user.ApiKey = model.ApiKey;
                if (PostJson(user, "api/user/insertapi") != null)
                {
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Error = "Invalid information was given.";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEventForm(EventModel model)
        {
            if (InSession())
            {
                // Get all eventTypes
                var eventTypes = GetAllEventTypes();
                // Set these eventTypes on the model. Needs to be done because
                // only the selected value from DropDownList is posed back, not the whole
                // list of EventType(s)
                model.EventTypes = GetOptionEventTypesList(eventTypes);

                if (ModelState.IsValid)
                {
                    Event tempEvent = PostJson(new Event(model.EventName, model.EventType,
                        model.EventLocation, model.EventDateTime, model.EventDescription,
                        model.EventMaxNumberOfCharacters, "116E0C0E-0035-44A9-BB22-4AE3E23127E5"), "api/guild/events/insert");
                    if (tempEvent != null)
                    {
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.Error = "Invalid information was given.";
                return View(model);
            } else
            {
                ViewBag.Error = "You aren't authorized to access this page.";
                return RedirectToAction("Index");
            }
        }

        private IEnumerable<string> GetAllEventTypes()
        {
            return Enum.GetNames(typeof(EventType.EventTypes)).ToList();
        }

        private IEnumerable<SelectListItem> GetOptionEventTypesList(IEnumerable<string> elements)
        {
            var eventTypeList = new List<SelectListItem>();
            foreach (var element in elements)
            {
                eventTypeList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }
            return eventTypeList;
        }

        private Boolean InSession()
        {
            return this.Session["Username"] != null;
        }

        private void EndSession()
        {
            FormsAuthentication.SignOut();
            this.Session.Abandon();
        }

        private void StartSession(User user)
        {
            this.Session["EmailAddress"] = user.EmailAddress;
            this.Session["Username"] = user.UserName;
        }

        //Not sure if this works
        //T is a generic type, need to test this more
        public T GetJson<T>(string urlSuffix, T returnType)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.BaseAddress = EndPoint;
                    var json = webClient.DownloadString(urlSuffix);
                    T t = JsonConvert.DeserializeObject<T>(json);
                    return t;
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public T PostJson<T>(T postObject, string urlSuffix)
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
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
    }
}