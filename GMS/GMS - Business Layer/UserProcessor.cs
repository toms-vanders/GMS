using GMS___Data_Access_Layer;
using GMS___Model;
using System;

namespace GMS___Business_Layer
{
    public class UserProcessor : UserProcessorIF
    {
        private UserAccess userAccess = new UserAccess();

        public Boolean InsertNewUser(string userName, string email, string password)
        {
            User userToBeAdded = new User(userName, email, password);
            return userAccess.InsertUser(userToBeAdded) == 1 ? true : false;
        }
        public User LogInUser(string emailAddress, string password)
        {
            User user = userAccess.GetUserFromDatabase(emailAddress);
            string saltedPassword = password + "salt";
            string hashedPassword = saltedPassword.GetHashCode() + "salt";
            string realPassword = hashedPassword.GetHashCode().ToString();
            if (user.Password == realPassword)
            {
                return user;
            }
            return null;
        }
    }
}
