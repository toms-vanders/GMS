using System;

namespace GMS___Model
{
    public class EventCharacter
    {
        public int EventID { get; set; }
        public string CharacterName { get; set; }
        public string CharacterRole { get; set; }
        public DateTime SignUpDateTime { get; set; }

        public EventCharacter(int eventID, string characterName, string characterRole, DateTime signUpDateTime)
        {
            EventID = eventID;
            CharacterName = characterName;
            CharacterRole = characterRole;
            SignUpDateTime = signUpDateTime;
        }

        public EventCharacter(int eventID, string characterName)
        {
            EventID = eventID;
            CharacterName = characterName;
        }

        public EventCharacter()
        {

        }
    }
}
