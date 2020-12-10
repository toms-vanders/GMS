using GMS___Business_Layer;
using GMS___Data_Access_Layer;
using GMS___Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GMS___Test
{
    [TestClass]
    public class EventProcessorTest
    {
        EventAccess ea;
        EventProcessor ep;
        Event testEvent;

        [TestInitialize]
        public void TestInitialize()
        {
            ea = new EventAccess();
            ep = new EventProcessor();

            string name = "Test Raid";
            string eventType = "Raid";
            string location = "266,070";
            DateTime date = new DateTime(2020, 12, 10, 12, 0, 0);
            string description = "Doing a full raid. Be available for at least 2 hours";
            int maxNumberOfCharacters = 25;
            string guildID = "99999999-9999-9999-9999-999999999999";

            testEvent = new Event(guildID, name, description, eventType, location, date, maxNumberOfCharacters);

        }

        [TestMethod]
        public void TestEventCreation()
        {
            Boolean InsertCompleted = false;
            Boolean TestThrewException = false;
            try
            {
                InsertCompleted = ep.InsertEvent(testEvent.Name, testEvent.EventType, testEvent.Location,
                testEvent.Date, testEvent.Description, testEvent.MaxNumberOfCharacters, testEvent.GuildID);
            } catch (Exception)
            {
                TestThrewException = true;
            } finally
            {
                //Cleanup
                int id = ea.getIdOfEvent(testEvent.Name);
                ea.DeleteEventByID(id);
            }

            Assert.IsTrue(InsertCompleted);
            Assert.IsFalse(TestThrewException);
        }

        [TestMethod]
        public void TestGettingEvents()
        {
            int firstResult = -1;
            int secondResult = -1;
            int thirdResult = -1;
            string name1 = "";
            string name2 = "";
            bool TestThrewException = false;
            try
            {
                ep.InsertEvent(testEvent.Name, testEvent.EventType, testEvent.Location,
                testEvent.Date, testEvent.Description, testEvent.MaxNumberOfCharacters, testEvent.GuildID);

                List<Event> firstSearch = (List<Event>)ep.GetAllGuildEvents(testEvent.GuildID);
                List<Event> secondSearch = (List<Event>)ep.GetAllGuildEventsByEventType(testEvent.GuildID, testEvent.EventType);
                List<Event> thirdSearch = (List<Event>)ep.GetAllGuildEventsByEventType(testEvent.GuildID, "Super Raid");

                firstResult = firstSearch.Count();
                secondResult = secondSearch.Count();
                thirdResult = thirdSearch.Count();
                name1 = firstSearch[0].Name;
                name2 = secondSearch[0].Name;
            } catch (Exception)
            {
                TestThrewException = true;
            } finally
            {
                //Cleanup
                int id = ea.getIdOfEvent(testEvent.Name);
                ea.DeleteEventByID(id);
            }

            Assert.AreEqual(1, firstResult);
            Assert.AreEqual(1, secondResult);
            Assert.AreEqual(0, thirdResult);
            Assert.AreEqual(testEvent.Name, name1);
            Assert.AreEqual(testEvent.Name, name2);
            Assert.IsFalse(TestThrewException);
        }

        [TestMethod]
        public void TestUpdate()
        {
            Event event1 = new Event();
            bool test1 = false;
            bool test2 = true;
            bool TestThrewException = false;
            try
            {
                ep.InsertEvent(testEvent.Name, testEvent.EventType, testEvent.Location,
                testEvent.Date, testEvent.Description, testEvent.MaxNumberOfCharacters, testEvent.GuildID);
                int id = ea.getIdOfEvent(testEvent.Name);
                Event eventToBeUpdated = ((List<Event>)ep.GetEventByID(id))[0];
                eventToBeUpdated.Location = "Update";
                test1 = ep.UpdateEvent(eventToBeUpdated.EventID, eventToBeUpdated.Name, eventToBeUpdated.EventType,
                    eventToBeUpdated.Location, eventToBeUpdated.Date, eventToBeUpdated.Description,
                    eventToBeUpdated.MaxNumberOfCharacters, eventToBeUpdated.GuildID, eventToBeUpdated.RowId);
                //Since the rowID wasnt updated this update should fail
                eventToBeUpdated.Location = "Another update";
                test2 = ep.UpdateEvent(eventToBeUpdated.EventID, eventToBeUpdated.Name, eventToBeUpdated.EventType,
                    eventToBeUpdated.Location, eventToBeUpdated.Date, eventToBeUpdated.Description,
                    eventToBeUpdated.MaxNumberOfCharacters, eventToBeUpdated.GuildID, eventToBeUpdated.RowId);
                event1 = ((List<Event>)ep.GetEventByID(id))[0];
            } catch (Exception)
            {
                TestThrewException = true;
            } finally
            {
                //Cleanup
                int id = ea.getIdOfEvent(testEvent.Name);
                ea.DeleteEventByID(id);
            }

            Assert.AreEqual("Update", event1.Location);
            Assert.IsTrue(test1);
            Assert.IsFalse(test2);
            Assert.IsFalse(TestThrewException);
        }
    }
}
