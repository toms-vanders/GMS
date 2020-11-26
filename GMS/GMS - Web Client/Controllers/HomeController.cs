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
        public static string ApiKey;

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
                string urlSuffix = "gw2api/characters/" + name;
                ViewBag.Character = GetJson<Character>(urlSuffix + "/core", new Character());
                ViewBag.Message = "Your character page.";
                ViewBag.Equipment = InitializeEquipment(GetJson<Equipments>(urlSuffix + "/equipment", new Equipments()));
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

        public ActionResult SearchEvents(string name)
        {
            if (this.Session["Username"] != null)
            {
                string urlSuffix = "gw2api/characters/" + name + "/core";
                ViewBag.Character = GetJson<Character>(urlSuffix, new Character());
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

        public ActionResult UpdateEventForm(int EventID,bool error = false)
        {
            if (InSession())
            {
                // Getting all event types needed for DropDownList
                var eventTypes = GetAllEventTypes();
                var model = new EventModel();
                model.EventTypes = GetOptionEventTypesList(eventTypes);
                // Get info about event
                EventProcessor processor = new EventProcessor();
                List<Event> events = (List<Event>)processor.GetEventByID(EventID);
                Event eventToBeUpdated = events[0];
                model.eventID = eventToBeUpdated.EventID;
                model.EventName = eventToBeUpdated.Name;
                model.EventType = eventToBeUpdated.EventType;
                model.EventLocation = eventToBeUpdated.Location;
                model.EventDateTime = eventToBeUpdated.Date;
                model.EventDescription = eventToBeUpdated.Description;
                model.EventMaxNumberOfCharacters = eventToBeUpdated.MaxNumberOfCharacters;
                model.rowID = eventToBeUpdated.RowId;
                if(error)
                {
                    ViewBag.Error = "Someone else already updated this event. If you still want to update please try again";
                }
                return View(model);
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEventForm(EventModel model)
        {
            if (InSession())
            {
                if (ModelState.IsValid)
                {
                    EventProcessor processor = new EventProcessor();
                    Boolean wasSuccessful = processor.UpdateEvent(model.eventID, model.EventName,
                        model.EventType, model.EventLocation, model.EventDateTime, model.EventDescription,
                        model.EventMaxNumberOfCharacters, "116E0C0E-0035-44A9-BB22-4AE3E23127E5", model.rowID);

                    if (wasSuccessful)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("UpdateEventForm", new { eventID = model.eventID, error = true});
                    }
                }
                ViewBag.Error = "There was a problem. Please try again";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "You aren't authorized to access this page.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(UserModel model)
        {
            if (ModelState.IsValid)
            {
                if (PostJson("api/user/signup", new User(model.UserName, model.EmailAddress, model.Password)) !=null)
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
                User user = PostJson("api/user/login", new User(model.EmailAddress, model.Password));
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
                if (PostJson("api/user/insertapi", user) != null)
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
                    Event tempEvent = PostJson("api/guild/events/insert", new Event(model.EventName, model.EventType,
                        model.EventLocation, model.EventDateTime, model.EventDescription,
                        model.EventMaxNumberOfCharacters, "116E0C0E-0035-44A9-BB22-4AE3E23127E5"));
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
            if (user.ApiKey == "") {
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
            }
            catch (WebException ex)
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
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public ArrayList InitializeEquipment(Equipments jsonList)
        {
            ArrayList equipment = new ArrayList();
            foreach (EquipmentSlot item in jsonList.Equipment)
            {
                try
                {
                    equipment.Add(GetJson<Item>("gw2api/items/" + item.Id, new Item()));
                } catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return equipment;
        }
    }
}