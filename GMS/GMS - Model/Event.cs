using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GMS___Model
{
    public class Event
    {
        public Event(string name, string eventType, string location, DateTime date, string description, int maxNumberOfCharacters, string guildId)
        {
            this.Name = name;
            this.EventType = eventType;
            this.Location = location;
            this.Date = date;
            this.Description = description;
            this.MaxNumberOfCharacters = maxNumberOfCharacters;
            this.GuildID = guildId;
            this.Participants = new ArrayList();
            this.WaitingList = new ArrayList();
        }

        public Event(int eventID, string guildId, string name, string description, string eventType, string location, DateTime date,  int maxNumberOfCharacters)
        {
            this.EventID = eventID;
            this.Name = name;
            this.EventType = eventType;
            this.Location = location;
            this.Date = date;
            this.Description = description;
            this.MaxNumberOfCharacters = maxNumberOfCharacters;
            this.GuildID = guildId;
            this.Participants = new ArrayList();
            this.WaitingList = new ArrayList();
        }

        public int EventID { get; set; }
        public string Name { get; set; }
        public string EventType { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int MaxNumberOfCharacters { get; set; }
        public string GuildID { get; set; }
        public ArrayList Participants { get; set; }
        public ArrayList WaitingList { get; set; }

    }
}
