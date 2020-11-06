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
            User userToBeAdded = new User(userName, email, HashPassword(password));
            return userAccess.InsertUser(userToBeAdded) == 1 ? true : false;
        }
        public User LogInUser(string emailAddress, string password)
        {
            User user = userAccess.GetUserFromDatabase(emailAddress);
            if (user.Password == HashPassword(password))
            {
                return user;
            }
            return null;
        }

        public string HashPassword(string password)
        {
            password += "salt";
            return password.GetHashCode().ToString();
        }
    }
}
