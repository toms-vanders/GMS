using System;
using System.Collections;
using GMS___Model;

namespace GMS___Model
{
    public class User
    {
        public User(string userName, string password, string email, string apiKey, ArrayList characters)
        {
            this.UserName = UserName;
            Password = password;
            this.EmailAddress = email;
            this.ApiKey = apiKey;
            this.Characters = characters;
        }
        public User(string userName, string email, string password, string apiKey)
        {
            this.UserName = userName;
            Password = password;
            this.EmailAddress = email;
            this.ApiKey = apiKey;
            this.Characters = new ArrayList();
        }

        public User(string userName, string email, string password)
        {
            this.UserName = userName;
            this.EmailAddress = email;
            this.Password = password;
            this.Characters = new ArrayList();
        }
        public string UserName { get; set; }
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
        public string EmailAddress { get; set; }
        public string ApiKey { get; set; }
        public ArrayList Characters { get; set; }
    }
}
