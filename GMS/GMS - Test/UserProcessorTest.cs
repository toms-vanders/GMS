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
            try
            {
                UserProcessor up = new UserProcessor();

                test1 = up.InsertNewUser("name1", "mail1", "password");
                test2 = up.InsertNewUser("name1", "mail2", "password");
                test3 = up.InsertNewUser("name2", "mail1", "password"); 
            }
            catch(Exception e)
            {
                Assert.AreEqual(1, 2,"Test threw exception");
            }
            finally
            {
                //CleanUp
                UserAccess userAccess = new UserAccess();
                userAccess.DeleteByName("name1");
                userAccess.DeleteByName("name2");
            }
            Assert.AreEqual(true, test1);
            Assert.AreEqual(false, test2);
            Assert.AreEqual(false, test3);
        }
        [TestMethod]
        public void TestLogInUser()
        {
            User user1 = null;
            User user2 = null;
            User user3 = null;
            User user4 = null;
            try
            {
                UserProcessor up = new UserProcessor();
                up.InsertNewUser("name", "mail@mail.com", "password");

                user1 = up.LogInUser("mail@mail.com", "password");
                user2 = up.LogInUser("Non existing email address", "password");
                user3 = up.LogInUser("mail@mail.com", "wrong password");
                user4 = up.LogInUser("mail@mail.com", "Password");
            }
            catch(Exception e)
            {
                Assert.AreEqual(1, 2, "Test threw exception");
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
        }

        [TestMethod]
        public void TestInsertApiKey()
        {
            bool test1 = false;
            bool test2 = true;
            try
            {
                UserProcessor up = new UserProcessor();
                up.InsertNewUser("name", "mail@mail.com", "password");

                test1 = up.InsertApiKey("mail@mail.com", "key");
                test2 = up.InsertApiKey("Non existing email address", "key");
            }
            catch(Exception e)
            {
                Assert.AreEqual(1, 2, "Test threw exception");
            }
            finally
            {
                //CleanUp
                UserAccess userAccess = new UserAccess();
                userAccess.DeleteByName("name");
            }
            Assert.AreEqual(true, test1);
            Assert.AreEqual(false, test2);
        }
    }
}
