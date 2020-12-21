using System;
using System.Collections.Generic;
using System.Text;

namespace GMS___Data_Access_Layer
{
    interface IEventCharacterWaitingListAccess
    {
        bool DeleteEventCharacterByEventIDAndCharacterName(int eventID, string characterName);
    }
}
