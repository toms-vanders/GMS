using GMS___Model;
using System.Collections.Generic;

namespace GMS___Data_Access_Layer
{
    interface IEventAccess
    {
        Event GetEventByID(int eventID);
        IEnumerable<Event> GetAllGuildEvents(string guildID);
        IEnumerable<Event> GetAllGuildEventsByEventType(string guidlID, string eventType);
        bool InsertEvent(Event guildEvent);
        bool UpdateEvent(Event guildEvent);
        bool DeleteEventByID(int eventID);
    }
}
