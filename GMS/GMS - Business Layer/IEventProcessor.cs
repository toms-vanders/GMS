using GMS___Model;
using System;
using System.Collections.Generic;

namespace GMS___Business_Layer
{
    interface IEventProcessor
    {
        Event GetEventByID(int eventID);
        IEnumerable<Event> GetAllGuildEvents(string guildID);
        IEnumerable<Event> GetAllGuildEventsByEventType(string guidlID, string eventType);
        IEnumerable<Event> GetGuildEventsByCharacterName(string guildID, string characterName);
        bool InsertEvent(string name, string eventType, string location, DateTime date, string description, int maxNumberOfCharacters, string guildID);
        bool UpdateEvent(int eventID, string name, string eventType, string location, DateTime date, string description, int maxNumberOfCharacters, string guildID, byte[] rowId);
        bool DeleteEventByID(int eventID);
        bool HasEventChangedRowVersion(int eventId, byte[] rowId);
    }
}
