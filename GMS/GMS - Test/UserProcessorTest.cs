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
            try
            {
                UserProcessor up = new UserProcessor();

                Boolean test1 = up.InsertNewUser("name1", "mail1", "password");
                Boolean test2 = up.InsertNewUser("name1", "mail2", "password");
                Boolean test3 = up.InsertNewUser("name2", "mail1", "password");

                Assert.AreEqual(true, test1);
                Assert.AreEqual(false, test2);
                Assert.AreEqual(false, test3);
            }
            catch(Exception e)
            {
                Assert.AreEqual(1, 2);
            }
            finally
            {
                //CleanUp
                UserAccess userAccess = new UserAccess();
                userAccess.DeleteByName("name1");
                userAccess.DeleteByName("name2");
            }
        }

        [TestMethod]
        public void TestLogInUser()
        {
            try
            {
                UserProcessor up = new UserProcessor();
                up.InsertNewUser("name", "mail@mail.com", "password");

                User user1 = up.LogInUser("mail@mail.com", "password");
                User user2 = up.LogInUser("Non existing email address", "password");
                User user3 = up.LogInUser("mail@mail.com", "wrong password");
                User user4 = up.LogInUser("mail@mail.com", "Password");

                Assert.AreEqual("name", user1.UserName);
                Assert.IsNull(user2);
                Assert.IsNull(user3);
                Assert.IsNull(user4);
            }
            catch(Exception e)
            {
                Assert.AreEqual(1, 2);
            }
            finally
            {
                //CleanUp
                UserAccess userAccess = new UserAccess();
                userAccess.DeleteByName("name");
            }
        }

        [TestMethod]
        public void TestInsertApiKey()
        {
            try
            {
                UserProcessor up = new UserProcessor();
                up.InsertNewUser("name", "mail@mail.com", "password");

                bool test1 = up.InsertApiKey("mail@mail.com", "key");
                bool test2 = up.InsertApiKey("Non existing email address", "key");

                Assert.AreEqual(true, test1);
                Assert.AreEqual(true, test2);
            }
            catch(Exception e)
            {
                Assert.AreEqual(1, 2);
            }
            finally
            {
                //CleanUp
                UserAccess userAccess = new UserAccess();
                userAccess.DeleteByName("name");
            }
        }
    }
}
