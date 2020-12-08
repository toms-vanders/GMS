using System;
using System.Collections;
using System.Collections.Generic;
using GMS___Model;

namespace GMS___Model
{
    public class User
    {
        public User()
        {
            this.UserName = "";
            this.EmailAddress = "";
            this.Password = "";
            this.ApiKey = "";
            this.Characters = new ArrayList();
            this.UserRole = "User";
            this.AccountCreated = DateTime.Now;
        }

        public User(string Username, string Password)
        {
            this.UserName = Username;
            this.EmailAddress = "";
            this.Password = Password;
            this.ApiKey = "";
            this.Characters = new ArrayList();
            this.UserRole = "User";
            this.AccountCreated = DateTime.Now;
        }
        public User(int UserID, string UserName, string Email, string Password, string ApiKey, string UserRole, DateTime accountCreated)
        {
            this.UserID = UserID;
            this.UserName = UserName;
            this.Password = Password;
            this.EmailAddress = Email;
            this.ApiKey = ApiKey;
            this.Characters = new ArrayList();
            this.UserRole = UserRole;
            this.AccountCreated = accountCreated;
        }
        public User(string UserName, string Email, string Password, string ApiKey, string UserRole, DateTime accountCreated)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.EmailAddress = Email;
            this.ApiKey = ApiKey;
            this.Characters = new ArrayList();
            this.UserRole = UserRole;
            this.AccountCreated = accountCreated;
        }
        public User(string userName, string password, string email, string apiKey, ArrayList characters, DateTime accountCreated)
        {
            this.UserName = userName;
            Password = password;
            this.EmailAddress = email;
            this.ApiKey = apiKey;
            this.Characters = characters;
            this.AccountCreated = accountCreated;
        }
        public User(string userName, string email, string password)
        {
            this.UserName = userName;
            this.EmailAddress = email;
            this.Password = password;
            this.Characters = new ArrayList();
            this.ApiKey = "";
            this.UserRole = "User";
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserID { get; set; }
        public string EmailAddress { get; set; }
        public string ApiKey { get; set; }
        public ArrayList Characters { get; set; }
        public string UserRole { get; set; }
        public DateTime AccountCreated { get; set; }
    }
}
