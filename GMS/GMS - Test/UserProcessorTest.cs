using GMS___Business_Layer;
using GMS___Data_Access_Layer;
using GMS___Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GMS___Test
{
    [TestClass]
    public class UserProcessorTest
    {
        [TestMethod]
        public void TestLogIn()
        {
            User user1 = null;
            User user2 = null;
            User user3 = null;
            User user4 = null;
            bool noExceptionWasThrown = true;
            try
            {
                UserProcessor up = new UserProcessor();
                up.InsertNewUser("name", "mail@mail.com", "password");
                up.InsertNewUser("name", "Non existing email address", "password");

                user1 = up.LogInUser("mail@mail.com", "password");
                user2 = up.LogInUser("Non existing email address", "password");
                user3 = up.LogInUser("mail@mail.com", "wrong password");
                user4 = up.LogInUser("mail@mail.com", "Password");
            } catch (Exception)
            {
                noExceptionWasThrown = false;
            } finally
            {
                //CleanUp
                UserAccess userAccess = new UserAccess();
                userAccess.DeleteByName("name");
            }
            Assert.AreEqual("name", user1.UserName);
            Assert.IsNull(user2);
            Assert.IsNull(user3);
            Assert.IsNull(user4);
            Assert.IsTrue(noExceptionWasThrown);
        }

        [TestMethod]
        public void TestInsertApiKey()
        {
            bool test1 = false;
            bool test2 = true;
            bool noExceptionWasThrown = true;
            string apikey = "";
            try
            {
                UserProcessor up = new UserProcessor();
                up.InsertNewUser("name", "mail@mail.com", "password");

                test1 = up.InsertApiKey("mail@mail.com", "key");
                test2 = up.InsertApiKey("Non existing email address", "key");
                User user = up.LogInUser("mail@mail.com", "password");
                apikey = user.ApiKey;
            } catch (Exception)
            {
                noExceptionWasThrown = false;
            } finally
            {
                //CleanUp
                UserAccess userAccess = new UserAccess();
                userAccess.DeleteByName("name");
            }
            Assert.IsTrue(test1);
            Assert.IsFalse(test2);
            Assert.AreEqual("key", apikey);
            Assert.IsTrue(noExceptionWasThrown);
        }

        [TestMethod]
        public void TestGetUserByEmail()
        {
            User user2 = null;
            string user1name = "";
            bool noExceptionWasThrown = true;
            try
            {
                UserProcessor up = new UserProcessor();
                up.InsertNewUser("name", "mail@mail.com", "password");

                User user1 = up.GetUserByEmail("mail@mail.com");
                user2 = up.GetUserByEmail("asd");
                user1name = user1.UserName;
            } catch (Exception)
            {
                noExceptionWasThrown = false;
            } finally
            {
                //CleanUp
                UserAccess userAccess = new UserAccess();
                userAccess.DeleteByName("name");
            }
            Assert.AreEqual("name", user1name);
            Assert.IsNull(user2);
            Assert.IsTrue(noExceptionWasThrown);
        }
    }
}
