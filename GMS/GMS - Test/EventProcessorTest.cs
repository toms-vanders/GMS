using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GMS___Model;
using GMS___Data_Access_Layer;
using GMS___Business_Layer;
using NodaTime;

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
        public void CreateValidEvent()
        {

            // Act and Assert
            Assert.IsTrue(ep.InsertEvent(testEvent.Name, testEvent.EventType, testEvent.Location, 
                testEvent.Date, testEvent.Description, testEvent.MaxNumberOfCharacters, testEvent.GuildID));
        }

        //[TestMethod]
        //public void GetGuildEventByEventIDFromDatabase()
        //{

        //    // Arrange
        //    List<Event> testEvents = new List<Event>();
        //    testEvents.Add(testEvent);

        //    // Act
        //    List<Event> returnList = (List<Event>)ep.GetAllGuildEvents(testEvent.GuildID);
        //    // Assert
        //    CollectionAssert.AreEqual(testEvents, returnList);
        //}

        [TestMethod]
        public void GetAllGuildEventListFromDatabase()
        {

            // Arrange
            List<Event> testEvents = new List<Event>();
            testEvents.Add(testEvent);

            // Act
            List<Event> returnList = (List<Event>)ep.GetAllGuildEvents(testEvent.GuildID);
            // Assert
            CollectionAssert.AreEqual(testEvents, returnList);
        }

        [TestMethod]
        public void GetAllGuildEventsByEventTypeFromDatabase()
        {

            // Arrange
            List<Event> testEvents = new List<Event>();
            testEvents.Add(testEvent);
            // Act
            List<Event> returnList = (List<Event>)ep.GetAllGuildEventsByEventType(testEvent.GuildID, testEvent.EventType);
            // Assert
            CollectionAssert.AreEqual(testEvents, returnList);
        }


        [TestMethod]
        public void DeleteExistingEvent()
        {

            // Arrange
            

            // Act

            // Assert
            Assert.IsTrue(ep.DeleteEventByID(2));
        }

        [TestCleanup]
        public void Cleanup()
        {
            //Should be implemented more specifically to delete test data from db
        }
    }
}
