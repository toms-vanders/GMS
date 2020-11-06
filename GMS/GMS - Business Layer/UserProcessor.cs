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
            User userToBeAdded = new User(userName, email, GetHashedPassword(password));
            return userAccess.InsertUser(userToBeAdded) == 1 ? true : false;
        }
        public User LogInUser(string emailAddress, string password)
        {
            User user = userAccess.GetUserFromDatabase(emailAddress);
            if (user.Password == GetHashedPassword(password))
            {
                return user;
            }
            return null;
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
