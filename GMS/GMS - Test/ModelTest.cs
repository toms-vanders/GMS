using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GMS___Model;
using NodaTime;

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

            User user = new User(userID, userName, email, password, apiKey, userRole);
            string hashedPassword = "password12345salt".GetHashCode().ToString();

            Assert.AreEqual(43, user.userID);
            Assert.AreEqual("Lemon", user.UserName);
            Assert.AreEqual("lemon88@ucn.dk", user.EmailAddress);
            Assert.AreEqual(hashedPassword, user.Password);
            Assert.AreEqual("apiKey12345", user.ApiKey);
            Assert.AreEqual("role", user.UserRole);
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
            string name = "GuildName";

            Guild guild = new Guild(name);

            Assert.AreEqual("GuildName", guild.name);
        }

        [TestMethod]
        public void TestEvent()
        {
            string name = "Random raid";
            string eventType = "Raids";
            string location = "37°14′0″N 115°48′30″W";
            LocalDate date = new LocalDate(2020,12,10);
            string description = "Raid description";
            int maxNumberOfCharacters = 20;

            Event event1 = new Event(name, eventType, location, date, description, maxNumberOfCharacters);
            int year = event1.date.Year;
            int month = event1.date.Month;
            int day = event1.date.Day;

            Assert.AreEqual("Random raid", event1.name);
            Assert.AreEqual("Raids", event1.eventType);
            Assert.AreEqual("37°14′0″N 115°48′30″W", event1.location);
            Assert.AreEqual(2020, year);
            Assert.AreEqual(12, month);
            Assert.AreEqual(10, day);
            Assert.AreEqual("Raid description", event1.description);
            Assert.AreEqual(20, event1.maxNumberOfCharacters);
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
