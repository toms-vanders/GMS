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

        public ActionResult CreateEventForm()
        {
            if (this.Session["Username"] != null)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEventForm(EventModel model)
        {
            if (this.Session["Username"] != null)
            {
                // Get all eventTypes
                var eventTypes = GetAllEventTypes();
                // Set these eventTypes on the model. Needs to be done because
                // only the selected value from DropDownList is posed back, not the whole
                // list of EventType(s)
                model.EventTypes = GetOptionEventTypesList(eventTypes);

                if (ModelState.IsValid)
                {
                    EventProcessor eventProcessor = new EventProcessor();
                    Boolean wasSuccessful = eventProcessor.InsertEvent(model.EventName, model.EventType,
                        model.EventLocation, model.EventDateTime, model.EventDescription,
                        model.EventMaxNumberOfCharacters, "116E0C0E-0035-44A9-BB22-4AE3E23127E5");

                    if (wasSuccessful)
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
    }
}