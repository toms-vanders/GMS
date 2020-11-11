using System;
using System.Collections.Generic;
using System.Text;
using GMS___Data_Access_Layer;
using GMS___Model;
using NodaTime;

namespace GMS___Business_Layer
{
    public class EventProcessor : IEventProcessor
    {

        private EventAccess eventAccess = new EventAccess();

        public IEnumerable<Event> GetEventByID(string eventID)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Event> GetAllGuildEvents(string guildID)
        {
            return eventAccess.GetAllGuildEvents(guildID);
        }
        public IEnumerable<Event> GetAllGuildEventsByEventType(string guidlID, string eventType)
        {
            return eventAccess.GetAllGuildEventsByEventType(guidlID, eventType);
        }
        
        public bool InsertEvent(string name, string eventType, string location, DateTime date, string description, int maxNumberOfCharacters, string guildID)
        {
            Event eventToBeAdded = new Event(name, eventType, location, date, description, maxNumberOfCharacters, guildID);
            return eventAccess.InsertEvent(eventToBeAdded);
        }
        
        public bool UpdateEvent(int eventID, string name, string eventType, string location, DateTime date, string description, int maxNumberOfCharacters, string guildID)
        {
            throw new NotImplementedException ();
        }

        public bool DeleteEventByID(int eventID)
        {
            return eventAccess.DeleteEventByID(eventID);
        }


    }
}
