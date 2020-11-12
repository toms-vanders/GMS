using System;
using System.Collections.Generic;
using System.Text;
using GMS___Model;

namespace GMS___Data_Access_Layer
{
    interface IEventAccess
    {
        IEnumerable<Event> GetEventByID(int eventID);
        IEnumerable<Event> GetAllGuildEvents(string guildID);
        IEnumerable<Event> GetAllGuildEventsByEventType(string guidlID, string eventType);
        bool InsertEvent(Event guildEvent);
        bool UpdateEvent(Event guildEvent);
        bool DeleteEventByID(int eventID);
    }
}
