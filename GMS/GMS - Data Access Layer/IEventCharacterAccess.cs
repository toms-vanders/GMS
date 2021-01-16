using GMS___Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMS___Data_Access_Layer
{
    interface IEventCharacterAccess
    {
        bool InsertEventCharacter(EventCharacter eventParticipant);
        bool ContainsEntry(int eventID, string characterName);
        bool DeleteEventCharacterByEventIDAndCharacterName(int eventID, string characterName);
        bool IsParticipantListFull(int eventID);
        bool MoveCharacterFromWaitingListToParticipantList(int eventID, string characterName);
        int ParticipantsInEvent(int eventID);
    }
}
