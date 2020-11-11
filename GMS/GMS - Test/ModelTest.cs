using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GMS___Model;
using NodaTime;
using System.Collections;

namespace GMS___Test
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void TestUser()
        {
            int userID = 43;
            string userName = "Lemon";
            string email = "lemon88@ucn.dk";
            string password = "password12345";
            string apiKey = "apiKey12345";
            string userRole = "role";
            ArrayList characters = new ArrayList();

            User user1 = new User(userID, userName, email, password, apiKey, userRole);
            User user2 = new User(userName, password, email, apiKey, characters);
            User user3 = new User(userName, email, password, apiKey, userRole);
            User user4 = new User(userName, email, password);

            Assert.AreEqual(43, user1.UserID);
            Assert.AreEqual("Lemon", user1.UserName);
            Assert.AreEqual("lemon88@ucn.dk", user1.EmailAddress);
            Assert.AreEqual("password12345", user1.Password);
            Assert.AreEqual("apiKey12345", user1.ApiKey);
            Assert.AreEqual("role", user1.UserRole);

            Assert.AreEqual("Lemon", user2.UserName);
            Assert.AreEqual("lemon88@ucn.dk", user2.EmailAddress);
            Assert.AreEqual("password12345", user2.Password);
            Assert.AreEqual("apiKey12345", user2.ApiKey);

            Assert.AreEqual("Lemon", user3.UserName);
            Assert.AreEqual("lemon88@ucn.dk", user3.EmailAddress);
            Assert.AreEqual("password12345", user3.Password);
            Assert.AreEqual("apiKey12345", user3.ApiKey);
            Assert.AreEqual("role", user3.UserRole);

            Assert.AreEqual("Lemon", user4.UserName);
            Assert.AreEqual("lemon88@ucn.dk", user4.EmailAddress);
            Assert.AreEqual("password12345", user4.Password);
            Assert.AreEqual("BASIC_USER", user4.UserRole);
        }

        [TestMethod]
        public void TestItem()
        {
            string name = "Sword";
            int value = 150;
            int quantity = 5;
            string description = "A sword";

            Item item = new Item(name,value,quantity,description);

            Assert.AreEqual("Sword", item.name);
            Assert.AreEqual(150, item.value);
            Assert.AreEqual(5, item.quantity);
            Assert.AreEqual("A sword", item.description);
        }

        [TestMethod]
        public void TestGuild()
        {
            string id = "116E0C0E-0035-44A9-BB22-4AE3E23127E5";
            string name = "GuildName";

            Guild guild = new Guild(id, name);

            Assert.AreEqual("116E0C0E-0035-44A9-BB22-4AE3E23127E5", guild.GuildID);
            Assert.AreEqual("GuildName", guild.Name);
           
        }

        [TestMethod]
        public void TestEvent()
        {
            int eventID = 50;
            string name = "Random raid";
            string eventType = "Raid";
            string location = "37°14′0″N 115°48′30″W";
            DateTime date = new DateTime(2020,12,10);
            string description = "Raid description";
            int maxNumberOfCharacters = 20;
            string guildID = "116E0C0E-0035-44A9-BB22-4AE3E23127E5";

            Event event1 = new Event(eventID, name, eventType, location, date, description, maxNumberOfCharacters, guildID);
            int year = event1.Date.Year;
            int month = event1.Date.Month;
            int day = event1.Date.Day;

            Assert.AreEqual(50, event1.EventID);
            Assert.AreEqual("Random raid", event1.Name);
            Assert.AreEqual("Raid", event1.EventType);
            Assert.AreEqual("37°14′0″N 115°48′30″W", event1.Location);
            Assert.AreEqual(2020, year);
            Assert.AreEqual(12, month);
            Assert.AreEqual(10, day);
            Assert.AreEqual("Raid description", event1.Description);
            Assert.AreEqual(20, event1.MaxNumberOfCharacters);
            Assert.AreEqual("116E0C0E-0035-44A9-BB22-4AE3E23127E5", event1.GuildID);
        }

        [TestMethod]
        public void TestCharacter()
        {
            string characterName = "Lime";
            string characterClass = "Thief";
            string email = "lime15@ucn.dk";
            int level = 29;
            string guildRank = "Rookie";

            Character character = new Character(characterName,characterClass,email,level,guildRank);

            Assert.AreEqual("Lime", character.characterName);
            Assert.AreEqual("Thief", character.characterClass);
            Assert.AreEqual("lime15@ucn.dk", character.email);
            Assert.AreEqual(29, character.level);
            Assert.AreEqual("Rookie", character.guildRank);
        }

        [TestMethod]
        public void TestAuction()
        {
            int auctionID = 20;
            int creatorID = 45;
            int eventID = 25;
            DateTime dateAndTimeOfCreation = new DateTime(2020, 12, 10, 12, 0, 0);
            int itemID = 10;
            decimal currentPrice = 150;
            int highestBidderID = 30;

            Auction auction = new Auction(auctionID,creatorID,eventID,dateAndTimeOfCreation,itemID,currentPrice,highestBidderID);
            int year = auction.DateAndTimeOfCreation.Year;
            int month = auction.DateAndTimeOfCreation.Month;
            int day = auction.DateAndTimeOfCreation.Day;
            int hour = auction.DateAndTimeOfCreation.Hour;
            int minute = auction.DateAndTimeOfCreation.Minute;
            int second = auction.DateAndTimeOfCreation.Second;

            Assert.AreEqual(20, auction.AuctionID);
            Assert.AreEqual(45, auction.CreatorID);
            Assert.AreEqual(25, auction.EventID);
            Assert.AreEqual(2020, year);
            Assert.AreEqual(12, month);
            Assert.AreEqual(10, day);
            Assert.AreEqual(12, hour);
            Assert.AreEqual(0, minute);
            Assert.AreEqual(0, second);
            Assert.AreEqual(10, auction.ItemID);
            Assert.AreEqual(150, auction.CurrentPrice);
            Assert.AreEqual(30, auction.HighestBidderID);
        }
    }
}
