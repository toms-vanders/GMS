using GMS___Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            int Id = 8;
            string Chat_link = "Chat Link";
            string Name = "Sword";
            string Icon = "Icon";
            string Description = "A sword";
            string Type = "Weapon";
            string Rarity = "Common";
            int Level = 1;
            int Vendor_value = 0;
            ArrayList Flags = new ArrayList();
            ArrayList Game_types = new ArrayList();
            ArrayList Restrictions = new ArrayList();

            Item item = new Item(Id, Chat_link, Name, Icon, Description, Type, Rarity, Level, Vendor_value, Flags, Game_types, Restrictions);

            Assert.AreEqual(8, item.Id);
            Assert.AreEqual("Chat Link", item.Chat_link);
            Assert.AreEqual("Sword", item.Name);
            Assert.AreEqual("Icon", item.Icon);
            Assert.AreEqual("A sword", item.Description);
            Assert.AreEqual("Weapon", item.Type);
            Assert.AreEqual("Common", item.Rarity);
            Assert.AreEqual(1, item.Level);
            Assert.AreEqual(0, item.Vendor_value);
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
        public void TestEventCharacter()
        {
            int eventID = 10;
            string characterName = "Something";
            string characterRole = "Healer";
            DateTime signUpDateTime = new DateTime(2020, 11, 1, 10, 30, 0);

            EventCharacter eventCharacter = new EventCharacter(eventID, characterName, characterRole, signUpDateTime);
            int year = eventCharacter.SignUpDateTime.Year;
            int month = eventCharacter.SignUpDateTime.Month;
            int day = eventCharacter.SignUpDateTime.Day;
            int hour = eventCharacter.SignUpDateTime.Hour;
            int minute = eventCharacter.SignUpDateTime.Minute;
            int second = eventCharacter.SignUpDateTime.Second;

            Assert.AreEqual(10, eventCharacter.EventID);
            Assert.AreEqual("Something", eventCharacter.CharacterName);
            Assert.AreEqual("Healer", eventCharacter.CharacterRole);
            Assert.AreEqual(2020, year);
            Assert.AreEqual(11, month);
            Assert.AreEqual(1, day);
            Assert.AreEqual(10, hour);
            Assert.AreEqual(30, minute);
            Assert.AreEqual(0, second);
        }

        [TestMethod]
        public void TestEvent()
        {
            int eventID = 50;
            string guildID = "116E0C0E-0035-44A9-BB22-4AE3E23127E5";
            string name = "Random raid";
            string description = "Raid description";
            string eventType = "Raid";
            string location = "37°14′0″N 115°48′30″W";
            DateTime date = new DateTime(2020, 12, 10);
            int maxNumberOfCharacters = 20;
            Byte[] rowId = new Byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

            Event event1 = new Event(eventID, guildID, name, description, eventType, location, date, maxNumberOfCharacters, rowId);
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
        public void TestEquipmentSlot()
        {
            int Id = 43;
            string Slot = "Slot";
            string Bound_to = "Bound to";
            ArrayList Dyes = new ArrayList();

            EquipmentSlot equipmentSlot = new EquipmentSlot(Id, Slot, Bound_to, Dyes);

            Assert.AreEqual(43, equipmentSlot.Id);
            Assert.AreEqual("Slot", equipmentSlot.Slot);
            Assert.AreEqual("Bound to", equipmentSlot.Bound_to);
        }

        [TestMethod]
        public void TestCharacter()
        {
            string name = "Lime";
            string race = "Human";
            string gender = "Male";
            string profession = "Thief";
            int level = 29;
            string guild = "0123456789";
            int age = 12;
            string created = "Yes";
            int deaths = 99999;
            string title = "Title";

            Character character = new Character(name, race, gender, profession, level, guild, age, created, deaths, title);

            Assert.AreEqual("Lime", character.Name);
            Assert.AreEqual("Human", character.Race);
            Assert.AreEqual("Male", character.Gender);
            Assert.AreEqual("Thief", character.Profession);
            Assert.AreEqual(29, character.Level);
            Assert.AreEqual("0123456789", character.Guild);
            Assert.AreEqual(12, character.Age);
            Assert.AreEqual("Yes", character.Created);
            Assert.AreEqual(99999, character.Deaths);
            Assert.AreEqual("Title", character.Title);
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

            Auction auction = new Auction(auctionID, creatorID, eventID, dateAndTimeOfCreation, itemID, currentPrice, highestBidderID);
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
