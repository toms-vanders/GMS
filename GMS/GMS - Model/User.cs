using System;
using System.Collections;
using GMS___Model;

namespace GMS___Model
{
    public class User
    {
        public User(string userName, string password, string email, string apiKey, ArrayList characters)
        {
            this.userName = userName;
            Password = password;
            this.email = email;
            this.apiKey = apiKey;
            this.characters = characters;
        }
        public User(int userID, string userName, string email, string password, string apiKey)
        {
            this.userID = userID;
            this.userName = userName;
            Password = password;
            this.email = email;
            this.apiKey = apiKey;
            this.characters = new ArrayList();
        }

        public User(int userID, string userName, string email, string password)
        {
            this.userID = userID;
            this.userName = userName;
            this.email = email;
            this.password = password;
            this.characters = new ArrayList();
        }
        public string userName { get; set; }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                string temp = value + "salt";
                password = temp.GetHashCode().ToString();
            }
        }
        public string email { get; set; }
        public string apiKey { get; set; }
        public int userID { get; set; }
        public ArrayList characters { get; set; }
    }
}
