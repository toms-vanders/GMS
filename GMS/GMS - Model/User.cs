﻿using System.Collections;

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
            this.UserRole = "BASIC_USER";
        }

        public User(string Email, string Password)
        {
            this.UserName = "";
            this.EmailAddress = Email;
            this.Password = Password;
            this.ApiKey = "";
            this.Characters = new ArrayList();
            this.UserRole = "BASIC_USER";
        }
        public User(int UserID, string UserName, string Email, string Password, string ApiKey, string UserRole)
        {
            this.UserID = UserID;
            this.UserName = UserName;
            this.Password = Password;
            this.EmailAddress = Email;
            this.ApiKey = ApiKey;
            this.Characters = new ArrayList();
            this.UserRole = UserRole;
        }
        public User(string UserName, string Email, string Password, string ApiKey, string UserRole)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.EmailAddress = Email;
            this.ApiKey = ApiKey;
            this.Characters = new ArrayList();
            this.UserRole = UserRole;
        }
        public User(string userName, string password, string email, string apiKey, ArrayList characters)
        {
            this.UserName = userName;
            Password = password;
            this.EmailAddress = email;
            this.ApiKey = apiKey;
            this.Characters = characters;
        }
        public User(string userName, string email, string password)
        {
            this.UserName = userName;
            this.EmailAddress = email;
            this.Password = password;
            this.Characters = new ArrayList();
            this.ApiKey = "";
            this.UserRole = "BASIC_USER";
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserID { get; set; }
        public string EmailAddress { get; set; }
        public string ApiKey { get; set; }
        public ArrayList Characters { get; set; }
        public string UserRole { get; set; }
    }
}
