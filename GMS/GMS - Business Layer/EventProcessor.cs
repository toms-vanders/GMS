using GMS___Data_Access_Layer;
using GMS___Model;
using System;
using System.Collections.Generic;

namespace GMS___Business_Layer
{
    public class EventProcessor : IEventProcessor
    {
        private EventAccess eventAccess = new EventAccess();

        public Event GetEventByID(int eventID)
        {
            return eventAccess.GetEventByID(eventID);
        }
        public IEnumerable<Event> GetAllGuildEvents(string guildID)
        {
            return eventAccess.GetAllGuildEvents(guildID);
        }
        public IEnumerable<Event> GetAllGuildEventsByEventType(string guidlID, string eventType)
        {
            return eventAccess.GetAllGuildEventsByEventType(guidlID, eventType);
        }
        public IEnumerable<Event> GetGuildEventsByCharacterName(string guildID, string characterName)
        {
            return eventAccess.GetGuildEventsByCharacterName(guildID, characterName);
        }

        public bool InsertEvent(string name, string eventType, string location, DateTime date, string description, int maxNumberOfCharacters, string guildID)
        {
            Event eventToBeAdded = new Event(guildID, name, description, eventType, location, date, maxNumberOfCharacters);
            return eventAccess.InsertEvent(eventToBeAdded);
        }

        public bool UpdateEvent(int eventID, string name, string eventType, string location, DateTime date, string description, int maxNumberOfCharacters, string guildID, byte[] rowId)
        {
            EventCharacterWaitingListProcessor ecwp = new EventCharacterWaitingListProcessor();
            Event eventToBeUpdated = new Event(eventID, guildID, name, description, eventType, location, date, maxNumberOfCharacters, rowId);
            bool wasUpdateSuccessful = eventAccess.UpdateEvent(eventToBeUpdated);
            ecwp.MovePeopleFromWaitingList(eventID);
            return wasUpdateSuccessful;
        }

        public bool DeleteEventByID(int eventID)
        {
            return eventAccess.DeleteEventByID(eventID);
        }


        public bool HasEventChangedRowVersion(int eventId, byte[] rowId)
        {
            return eventAccess.HasEventChangedRowVersion(eventId, rowId);
        }
    }
}
