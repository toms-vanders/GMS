using GMS___Data_Access_Layer;
using GMS___Model;
using System;

namespace GMS___Business_Layer
{
    public class UserProcessor
    {
        private UserAccess userAccess = new UserAccess();

        public Boolean InsertNewUser(string userName, string email, string password)
        {
            User userToBeAdded = new User(userName, email, password);
            return userAccess.InsertUser(userToBeAdded) == 1 ? true : false; 
        }
    }
}
