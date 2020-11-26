using GMS___Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Text;


namespace GMS___Business_Layer
{
    public class EventCharacterWaitingListProcessor
    {
        private EventCharacterWaitingListAccess eventCharacterWaitingListAccess = new EventCharacterWaitingListAccess();

        public bool DeleteEventCharacterByEventIDAndCharacterName(int eventID, string characterName)
        {
            return eventCharacterWaitingListAccess.DeleteEventCharacterByEventIDAndCharacterName(eventID, characterName);
        }
    }
}
