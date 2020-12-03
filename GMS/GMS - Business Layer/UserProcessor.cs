using GMS___Data_Access_Layer;
using GMS___Model;
using System;

namespace GMS___Business_Layer
{
    public class UserProcessor : UserProcessorIF
    {
        private UserAccess userAccess = new UserAccess();

        public User GetUserByEmail(string email)
        {
            return userAccess.GetUserFromDatabase(email);
        }
        public User InsertNewUser(string userName, string email, string password)
        {
            User userToBeAdded = new User(userName, email, GetHashedPassword(password));
            userAccess.InsertUser(userToBeAdded);
            return userToBeAdded;
        }
        public User GetUserByUsername(string email)
        {
            return userAccess.GetUserFromDatabaseWithUsername(email);
        }
        public User LogInUser(string username, string password)
        {
            User user = userAccess.GetUserFromDatabaseWithUsername(username);
            if (user is null)
            {
                return null;
            }
            if (user.Password == GetHashedPassword(password))
            {
                user.UserName = username;
                return user;
            }
            return null;
        }
        public Boolean InsertApiKey(string emailAddress, string apiKey)
        {
            User user = userAccess.GetUserFromDatabase(emailAddress);
            if (user is null)
            {
                return false;
            }
            user.ApiKey = apiKey;
            return userAccess.UpdateUser(user) == 1 ? true : false;
        }
        public string GetHashedPassword(string password)
        {
            password += "salt";
            return GetHashCode(password).ToString();
        }

        public int GetHashCode(string original)
        {
            long sum = 0, mul = 1;
            for (int i = 0; i < original.Length; i++)
            {
                mul = (i % 4 == 0) ? 1 : mul * 256;
                sum += original[i] * mul;
            }
            return (int)(Math.Abs(sum) % 2147483647);
        }
    }
}
