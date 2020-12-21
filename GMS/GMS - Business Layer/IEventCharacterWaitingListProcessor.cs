using System;
using System.Collections.Generic;
using System.Text;

namespace GMS___Business_Layer
{
    interface IEventCharacterWaitingListProcessor
    {
        bool DeleteEventCharacterByEventIDAndCharacterName(int eventID, string characterName);
    }
}
