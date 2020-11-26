﻿using GMS___Data_Access_Layer;
using GMS___Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMS___Business_Layer
{
    public class EventCharacterProcessor
    {
        private EventCharacterAccess eventCharacterAccess = new EventCharacterAccess();
        public Boolean JoinEvent(int eventID, string characterName, string role, DateTime signUpDateTime)
        {
            EventCharacter eventCharacterToBeAdded = new EventCharacter(eventID, characterName, role, signUpDateTime);
            return eventCharacterAccess.InsertEventCharacter(eventCharacterToBeAdded);
        }
    }
}