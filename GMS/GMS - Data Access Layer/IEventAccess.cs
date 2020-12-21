using GMS___Model;
using System.Collections.Generic;

namespace GMS___Data_Access_Layer
{
    interface IEventAccess
    {
        Event GetEventByID(int eventID);
        IEnumerable<Event> GetAllGuildEvents(string guildID);
        IEnumerable<Event> GetAllGuildEventsByEventType(string guidlID, string eventType);
        IEnumerable<Event> GetGuildEventsByCharacterName(string guildID, string characterName);
        bool HasEventChangedRowVersion(int eventID, byte[] currentRowId);
        bool InsertEvent(Event guildEvent);
        bool UpdateEvent(Event guildEvent);
        bool DeleteEventByID(int eventID);
        int GetIdOfEvent(string eventName);
    }
}
