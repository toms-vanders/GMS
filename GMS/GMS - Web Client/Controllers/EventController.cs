using GMS___Business_Layer;
using GMS___Model;
using GMS___Web_Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GMS___Web_Client.Controllers
{
    public class EventController : AuthController
    {
        // GET: Event

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
            return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index", "Home");
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
                        return RedirectToAction("Index", "Home");
                    }
                }
                ViewBag.Error = "Invalid information was given.";
                return View(model);
            } else
            {
                ViewBag.Error = "You aren't authorized to access this page.";
                return RedirectToAction("Index", "Home");
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

        public ActionResult UpdateEventForm(int EventID, bool error = false)
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
                if (error)
                {
                    ViewBag.Error = "Someone else already updated this event. If you still want to update please try again";
                }
                return View(model);
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index", "Home");
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
                        return RedirectToAction("Index","Home");
                    } else
                    {
                        return RedirectToAction("UpdateEventForm","Event", new { eventID = model.eventID, error = true });
                    }
                }
                ViewBag.Error = "There was a problem. Please try again";
                return RedirectToAction("Index", "Home");
            } else
            {
                ViewBag.Error = "You aren't authorized to access this page.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}