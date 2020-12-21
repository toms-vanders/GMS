using GMS___Data_Access_Layer;
using GMS___Model;
using System;

namespace GMS___Business_Layer
{
    public class UserProcessor : IUserProcessor
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
        public User GetUserByUsername(string name)
        {
            return userAccess.GetUserFromDatabaseWithUsername(name);
        }
        public User LogInUser(string username, string password)
        {
            User user = userAccess.GetUserFromDatabaseWithUsername(username);
            if (user is null)
            {
                return null;
            }
            try
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    user.UserName = username;
                    return user;
                }
            } catch (Exception)
            {
                return null;
            }

            return null;
        }
        public bool InsertApiKey(string emailAddress, string apiKey)
        {
            User user = userAccess.GetUserFromDatabase(emailAddress);
            if (user is null)
            {
                return false;
            }
            user.ApiKey = apiKey;
            return userAccess.UpdateUser(user) == 1 ? true : false;
        }
        public bool ChangeUsername(string oldUsername, string newUsername, string password)
        {
            try
            {
                User user = userAccess.GetUserFromDatabaseWithUsername(oldUsername);
                if (VerifyPassword(password, user.Password))
                {
                    user.UserName = newUsername;
                    return userAccess.UpdateUser(user) == 1 ? true : false;
                }
            }
            catch (Exception)
            {
                return false; // Need to catch the exception
            }

            return false;
            
        }

        public bool ChangeEmail(string username, string newEmailAddress, string password)
        {
            try
            {
                User user = userAccess.GetUserFromDatabaseWithUsername(username);
                if (VerifyPassword(password, user.Password))
                {
                    user.EmailAddress = newEmailAddress;
                    return userAccess.UpdateUser(user) == 1 ? true : false;
                }
            }
            catch (Exception)
            {
                return false; // Need to catch the exception
            }

            return false;
        }

        public bool ChangePassword(string username, string currentPassword, string newPassword)
        {
            try
            {
                User user = userAccess.GetUserFromDatabaseWithUsername(username);
                if (VerifyPassword(currentPassword, user.Password))
                {
                    user.Password = GetHashedPassword(newPassword);
                    return userAccess.UpdateUser(user) == 1 ? true : false;
                }
            }
            catch (Exception)
            {
                return false; // Need to catch the exception
            }

            return false;
        }

        public string GetHashedPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
