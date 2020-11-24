using System;
using System.Collections.Generic;
using System.Text;

namespace GMS___Model
{
    public class EventCharacter
    {
        public int EventID { get; set; }
        public string CharacterName { get; set; }
        public string Role { get; set; }

        public EventCharacter(int eventID, string characterName, string role)
        {
            EventID = eventID;
            CharacterName = characterName;
            Role = role;
        }

        public EventCharacter()
        {

        }
    }
}
