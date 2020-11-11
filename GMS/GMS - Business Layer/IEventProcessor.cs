﻿using System;
using System.Collections.Generic;
using System.Text;
using GMS___Model;
using NodaTime;

namespace GMS___Business_Layer
{
    interface IEventProcessor
    {
        IEnumerable<Event> GetEventByID(string eventID);
        IEnumerable<Event> GetAllGuildEvents(string guildID);
        IEnumerable<Event> GetAllGuildEventsByEventType(string guidlID, string eventType);
        bool InsertEvent(string name, string eventType, string location, LocalDate date, string description, int maxNumberOfCharacters, string guildI);
        bool UpdateEvent(int eventID, string name, string eventType, string location, LocalDate date, string description, int maxNumberOfCharacters, string guildI);
        bool DeleteEventByID(int eventID);
     
    }
}