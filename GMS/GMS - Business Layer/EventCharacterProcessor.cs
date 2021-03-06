using GMS___Data_Access_Layer;
using GMS___Model;
using System;

namespace GMS___Business_Layer
{
    public class EventCharacterProcessor : IEventCharacterProcessor
    {
        private EventCharacterAccess eventCharacterAccess = new EventCharacterAccess();
        public Boolean JoinEvent(int eventID, string characterName, string role, DateTime signUpDateTime)
        {
            EventCharacter eventCharacterToBeAdded = new EventCharacter(eventID, characterName, role, signUpDateTime);
            return eventCharacterAccess.InsertEventCharacter(eventCharacterToBeAdded);
        }

        public bool DeleteEventCharacterByEventIDAndCharacterName(int eventID, string characterName)
        {
            return eventCharacterAccess.DeleteEventCharacterByEventIDAndCharacterName(eventID, characterName);
        }

        public bool ContainsEntry(int eventId, string characterName)
        {
            return eventCharacterAccess.ContainsEntry(eventId, characterName);
        }

        public int ParticipantsInEvent(int eventID)
        {
            return eventCharacterAccess.ParticipantsInEvent(eventID);
        }
    }
}
