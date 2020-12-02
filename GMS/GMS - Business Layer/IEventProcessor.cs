using GMS___Model;
using System;
using System.Collections.Generic;

namespace GMS___Business_Layer
{
    interface IEventProcessor
    {
        IEnumerable<Event> GetEventByID(int eventID);
        IEnumerable<Event> GetAllGuildEvents(string guildID);
        IEnumerable<Event> GetAllGuildEventsByEventType(string guidlID, string eventType);
        bool InsertEvent(string name, string eventType, string location, DateTime date, string description, int maxNumberOfCharacters, string guildI);
        bool UpdateEvent(int eventID, string name, string eventType, string location, DateTime date, string description, int maxNumberOfCharacters, string guildI, byte[] rowId);
        bool DeleteEventByID(int eventID);

    }
}
