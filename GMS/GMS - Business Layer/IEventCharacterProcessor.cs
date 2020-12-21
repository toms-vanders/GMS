using System;
using System.Collections.Generic;
using System.Text;

namespace GMS___Business_Layer
{
    interface IEventCharacterProcessor
    {
        Boolean JoinEvent(int eventID, string characterName, string role, DateTime signUpDateTime);
        bool DeleteEventCharacterByEventIDAndCharacterName(int eventID, string characterName);
        bool ContainsEntry(int eventId, string characterName);
        int ParticipantsInEvent(int eventID);
    }
}
