using GMS___Business_Layer;
using GMS___Model;
using GMS___Web_Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GMS___Web_Client.Controllers
{
    public class EventController : AuthController
    {
        // GET: Event

        public ActionResult CreateEventForm(string name)
        {
            if (InSession())
            {
                string urlSuffix = "gw2api/characters/" + name + "/core";
                ViewBag.Character = GetJson<Character>(urlSuffix);
                // Getting all event types needed for DropDownList
                var eventTypes = GetAllEventTypes();
                var model = new EventModel();
                // Get list of SelectListItem(s)
                model.EventTypes = GetOptionEventTypesList(eventTypes);
                model.GuildID = ViewBag.Character.Guild;
                ViewBag.Message = "Creating event.";
                ViewBag.UserToken = Session["UserToken"];
                return View(model);
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SearchEvents(string name)
        {
            if (this.Session["Username"] != null)
            {
                var error = TempData["ErrorMessage"] as string;
                ViewBag.Error = error;
                string urlSuffix = "gw2api/characters/" + name + "/core";
                ViewBag.Character = GetJson<Character>(urlSuffix);
                // Getting all event types needed for DropDownList
                var eventTypes = GetAllEventTypes();
                var model = new EventModel();
                // Get list of SelectListItem(s)
                model.EventTypes = GetOptionEventTypesList(eventTypes);
                ViewBag.Message = "Find new events";
                ViewBag.UserToken = Session["UserToken"];
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
                    Event tempEvent = PostJson("api/guild/events/insert", new Event(model.GuildID, model.EventName,
                        model.EventDescription, model.EventType, model.EventLocation, model.EventDateTime,
                        model.EventMaxNumberOfCharacters));
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

        public ActionResult UpdateEventForm(string name, int eventID, bool error = false)
        {
            if (InSession())
            {
                this.Session["characterName"] = name;
                string urlSuffix = "gw2api/characters/" + name + "/core";
                ViewBag.Character = GetJson<Character>(urlSuffix);
                ViewBag.UserToken = Session["UserToken"];
                // Getting all event types needed for DropDownList
                var eventTypes = GetAllEventTypes();
                var model = new EventModel();
                model.EventTypes = GetOptionEventTypesList(eventTypes);
                model.GuildID = ViewBag.Character.Guild;
                // Get info about event
                EventProcessor processor = new EventProcessor();
                Event events = processor.GetEventByID(eventID);
                if (events is null)
                {
                    TempData["ErrorMessage"] = "This Event no longer exists";
                    return RedirectToAction("SearchEvents", "Event", new { name = this.Session["characterName"] });
                } else
                {
                    Event eventToBeUpdated = events;
                    model.eventID = eventToBeUpdated.EventID;
                    model.EventName = eventToBeUpdated.Name;
                    model.EventType = eventToBeUpdated.EventType;
                    model.EventLocation = eventToBeUpdated.Location;
                    model.EventDateTime = eventToBeUpdated.Date;
                    model.EventDescription = eventToBeUpdated.Description;
                    model.EventMaxNumberOfCharacters = eventToBeUpdated.MaxNumberOfCharacters;
                    model.rowID = eventToBeUpdated.RowId;
                    model.GuildID = eventToBeUpdated.GuildID;
                    if (error)
                    {
                        ViewBag.Error = "The record you attempted to edit was modified by another user after you got the original value. " +
                            "The edit operation was cancelled and the current values in the database have been displayed. " +
                            "If you still want to edit this record, click the Update button again. Otherwise click the Back to List hyperlink";
                    }
                    return View(model);
                }
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
                    if (model.EventMaxNumberOfCharacters < JsonConvert.DeserializeObject<int>(getNumberOfSignedParticipants(model.eventID)))
                    {
                        var eventTypes = GetAllEventTypes();
                        model.EventTypes = GetOptionEventTypesList(eventTypes);
                        ViewBag.Error = "You cant set the maximum number of participants to below the current number of participants.";
                        return View(model);
                    }

                    Event tempEvent = PostJson("api/guild/events/update", new Event(model.eventID, model.GuildID, model.EventName,
                        model.EventDescription, model.EventType, model.EventLocation, model.EventDateTime,
                        model.EventMaxNumberOfCharacters, model.rowID));

                    if (tempEvent != null)
                    {
                        return RedirectToAction("SearchEvents", "Event", new { name = this.Session["characterName"] });
                    } else
                    {
                        return RedirectToAction("UpdateEventForm", "Event", new { name = this.Session["characterName"], eventID = model.eventID, error = true });
                    }
                } else
                {
                    ViewBag.Error = "Invalid information was given.";
                    var eventTypes = GetAllEventTypes();
                    model.EventTypes = GetOptionEventTypesList(eventTypes);
                    return View(model);
                }
            } else
            {
                ViewBag.Error = "You aren't authorized to access this page.";
                return RedirectToAction("Index", "Home");
            }
        }

        public string getNumberOfSignedParticipants(int eventId)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.BaseAddress = EndPoint;
                    webClient.Encoding = System.Text.Encoding.UTF8;
                    webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    webClient.Headers.Add("Authorization", this.Session["UserToken"].ToString());
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                    return webClient.DownloadString("api/guild/events/" + eventId + "/participants");
                }
            } catch (WebException ex)
            {
                throw ex;
            }
        }
    }
}