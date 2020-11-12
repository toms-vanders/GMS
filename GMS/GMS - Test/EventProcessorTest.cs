using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GMS___Model;
using GMS___Data_Access_Layer;
using GMS___Business_Layer;
using NodaTime;
using System.Linq;

namespace GMS___Test
{
    /// <summary>
    /// Summary description for EventProcessorTest
    /// </summary>
    [TestClass]
    public class EventProcessorTest
    {
        EventProcessor ep;
        Event testEvent;

        [TestInitialize]
        public void TestInitialize()
        {
            ep = new EventProcessor();
            
            string name = "Test Raid";
            string eventType = "Test Event";
            string location = "266,070";
            DateTime date = new DateTime(2020, 12, 10, 12, 0, 0);
            string description = "Doing a full raid. Be available for at least 2 hours";
            int maxNumberOfCharacters = 25;
            string guildID = "99999999-9999-9999-9999-999999999999";

            testEvent = new Event(name, eventType, location, date, description, maxNumberOfCharacters, guildID);

        }

        [TestMethod]
        public void EventProcessorIntegrationTest()
        {
            CreateValidEvent();
            GetAllGuildEventListFromDatabase();
            GetAllGuildEventsByEventTypeFromDatabase();
            GetGuildEventByEventIDFromDatabase();
            DeleteExistingEvent();
        }

        public void CreateValidEvent()
        {

            // Act and Assert
            Assert.IsTrue(ep.InsertEvent(testEvent.Name, testEvent.EventType, testEvent.Location, 
                testEvent.Date, testEvent.Description, testEvent.MaxNumberOfCharacters, testEvent.GuildID));
        }

        public void GetAllGuildEventListFromDatabase()
        {

            // Arrange

            // Act
            List<Event> returnList = (List<Event>)ep.GetAllGuildEvents(testEvent.GuildID);
            Event returnedEvent = returnList.FirstOrDefault();
            // Assert
            Assert.AreEqual(testEvent.GuildID, returnedEvent.GuildID);
        }

        public void GetAllGuildEventsByEventTypeFromDatabase()
        {

            // Arrange

            // Act
            List<Event> returnList = (List<Event>)ep.GetAllGuildEventsByEventType(testEvent.GuildID, testEvent.EventType);
            Event returnedEvent = returnList.FirstOrDefault();
            // Assert
            Assert.AreEqual(testEvent.EventType, returnedEvent.EventType);
        }

        public void GetGuildEventByEventIDFromDatabase()
        {
            // Assert
            List<Event> returnListByGuild = (List<Event>)ep.GetAllGuildEvents(testEvent.GuildID);
            Event testEventWithID = returnListByGuild.FirstOrDefault();

            // Act
            List<Event> returnListByID = (List<Event>)ep.GetEventByID(testEventWithID.EventID);
            Event returnedEvent = returnListByID.FirstOrDefault();
            // Assert
            Assert.AreEqual(returnedEvent.EventID, testEventWithID.EventID);
        }


        public void DeleteExistingEvent()
        {

            // Arrange
            List<Event> returnListByGuild = (List<Event>)ep.GetAllGuildEvents(testEvent.GuildID);
            Event testEventWithID = returnListByGuild.FirstOrDefault();

            // Act and Assert

            Assert.IsTrue(ep.DeleteEventByID(testEventWithID.EventID));
        }

        [TestCleanup]
        public void Cleanup()
        {
            //Should be implemented more specifically to delete test data from db
        }
    }
}
