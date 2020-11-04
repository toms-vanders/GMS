using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GMS___Model
{
    class Event
    {
        public Event(string name, EventType eventType, string location, LocalDate date, string description, int maxNumberOfCharacters)
        {
            this.name = name;
            this.eventType = eventType.ToString();
            this.location = location;
            this.date = date;
            this.description = description;
            this.maxNumberOfCharacters = maxNumberOfCharacters;
            this.participants = new ArrayList();
            this.waitingList = new ArrayList();
        }
        public string name { get; set; }
        public string eventType { get; set; }
        public string location { get; set; }
        public LocalDate date { get; set; }
        public string description { get; set; }
        public int maxNumberOfCharacters { get; set; }
        public ArrayList participants { get; set; }
        public ArrayList waitingList { get; set; }

    }
}
