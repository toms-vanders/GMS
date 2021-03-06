using GMS___Business_Layer;
using GMS___Data_Access_Layer;
using GMS___Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GMS___Test
{
    [TestClass]
    public class EventCharacterTest
    {
        EventAccess ea;
        EventProcessor ep;
        EventCharacterAccess eca;
        EventCharacterProcessor ecp;
        Event testEvent;
        Character testCharacter;

        [TestInitialize]
        public void TestInitialize()
        {
            ea = new EventAccess();
            ep = new EventProcessor();
            eca = new EventCharacterAccess();
            ecp = new EventCharacterProcessor();

            string eventName = "Test Raid";
            string eventType = "Raid";
            string location = "266,070";
            DateTime date = new DateTime(2020, 12, 10, 12, 0, 0);
            string description = "Doing a full raid. Be available for at least 2 hours";
            int maxNumberOfCharacters = 25;
            string guildID = "99999999-9999-9999-9999-999999999999";

            testEvent = new Event(guildID, eventName, description, eventType, location, date, maxNumberOfCharacters);

            string characterName = "Bob";
            string race = "Demon";
            string gender = "Both";
            string profession = "Janitor";
            int level = 95;
            string guild = "A guild";
            int age = 900;
            string created = "Yes";
            int deaths = 1;
            string title = "Title";

            testCharacter = new Character(characterName, race, gender, profession, level, guild, age, created, deaths, title);

            ep.InsertEvent(testEvent.Name, testEvent.EventType, testEvent.Location,
                testEvent.Date, testEvent.Description, testEvent.MaxNumberOfCharacters, testEvent.GuildID);
            testEvent.EventID = ea.GetIdOfEvent(testEvent.Name);
        }
        [TestCleanup]
        public void TestCleanup()
        {
            ea.DeleteEventByID(testEvent.EventID);
        }


        [TestMethod]
        public void TestJoin()
        {
            bool NoReasonThisShouldBeTrue = true;
            bool JoinCompleted = false;
            bool TestThrewException = false;
            string eventName = "";
            try
            {
                NoReasonThisShouldBeTrue = ecp.ContainsEntry(testEvent.EventID, testCharacter.Name);
                ecp.JoinEvent(testEvent.EventID, testCharacter.Name, "AFK", new DateTime(2020, 12, 24));
                JoinCompleted = ecp.ContainsEntry(testEvent.EventID, testCharacter.Name);
                List<Event> events = (List<Event>)ep.GetGuildEventsByCharacterName(testEvent.GuildID, testCharacter.Name);
                eventName = (events[0]).Name;
            }catch (Exception)
            {
                TestThrewException = true;
            }finally
            {
                //Cleanup
            }

            Assert.AreEqual("Test Raid", eventName);
            Assert.IsFalse(NoReasonThisShouldBeTrue);
            Assert.IsTrue(JoinCompleted);
            Assert.IsFalse(TestThrewException);
        }
        [TestMethod]
        public void TestNumberOfParticipants()
        {
            int beforeJoin = -1;
            int afterJoin = -1;
            bool TestThrewException = false;
            try
            {
                beforeJoin = ecp.ParticipantsInEvent(testEvent.EventID);
                ecp.JoinEvent(testEvent.EventID, testCharacter.Name, "AFK", new DateTime(2020, 12, 24));
                afterJoin = ecp.ParticipantsInEvent(testEvent.EventID);

            }catch (Exception)
            {
                TestThrewException = true;
            }finally
            {
                //Cleanup
            }

            Assert.AreEqual(0, beforeJoin);
            Assert.AreEqual(1, afterJoin);
            Assert.IsFalse(TestThrewException);
        }
    }
}
