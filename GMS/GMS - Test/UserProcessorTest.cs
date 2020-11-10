using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GMS___Business_Layer;
using System.Net.Mail;
using GMS___Model;
using GMS___Data_Access_Layer;

namespace GMS___Test
{
    [TestClass]
    public class UserProcessorTest
    {
        [TestMethod]
        public void TestInsertNewUser()
        {
            Boolean test1 = false;
            Boolean test2 = true;
            Boolean test3 = true;
            Boolean noExceptionWasThrown = true;
            try
            {
                UserProcessor up = new UserProcessor();

                test1 = up.InsertNewUser("name1", "mail1", "password");
                test2 = up.InsertNewUser("name1", "mail2", "password");
                test3 = up.InsertNewUser("name2", "mail1", "password"); 
            }
            catch(Exception e)
            {
                noExceptionWasThrown = false;
            }
            finally
            {
                //CleanUp
                UserAccess userAccess = new UserAccess();
                userAccess.DeleteByName("name1");
                userAccess.DeleteByName("name2");
            }
            Assert.IsTrue(test1);
            Assert.IsFalse(test2);
            Assert.IsFalse(test3);
            Assert.IsTrue(noExceptionWasThrown);
        }
        [TestMethod]
        public void TestLogInUser()
        {
            User user1 = null;
            User user2 = null;
            User user3 = null;
            User user4 = null;
            Boolean noExceptionWasThrown = true;
            try
            {
                UserProcessor up = new UserProcessor();
                up.InsertNewUser("name", "mail@mail.com", "password");
                up.InsertNewUser("name", "Non existing email address", "password");

                user1 = up.LogInUser("mail@mail.com", "password");
                user2 = up.LogInUser("Non existing email address", "password");
                user3 = up.LogInUser("mail@mail.com", "wrong password");
                user4 = up.LogInUser("mail@mail.com", "Password");
            }
            catch(Exception e)
            {
                noExceptionWasThrown = false;
            }
            finally
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
            Boolean test1 = false;
            Boolean test2 = true;
            Boolean noExceptionWasThrown = true;
            String apikey = "";
            try
            {
                UserProcessor up = new UserProcessor();
                up.InsertNewUser("name", "mail@mail.com", "password");

                test1 = up.InsertApiKey("mail@mail.com", "key");
                test2 = up.InsertApiKey("Non existing email address", "key");
                User user = up.LogInUser("mail@mail.com", "password");
                apikey = user.ApiKey;
            }
            catch(Exception e)
            {
                noExceptionWasThrown = false;
            }
            finally
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
    }
}
