using Dapper;
using GMS___Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GMS___Data_Access_Layer
{
    public class UserAccess : UserAccessIF
    {

        public IEnumerable<User> GetUsersFromDatabase()
        {
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                IEnumerable<User> users = conn.Query<User>("SELECT * FROM Users");
                return users;
            }
        }

        public User GetUserFromDatabase(String emailAddress)
        {
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                List<User> users = conn.Query<User>("SELECT userID, userName, emailAddress, password, apiKey, userRole, accountCreated FROM Users where emailAddress in @emails", new { emails = new[] { emailAddress } }).ToList();
                if (users.Count != 1)
                {
                    return (User)null;
                } else
                {
                    //users[0].EmailAddress = emailAddress;
                    return users[0];
                }
            }
        }

        public User GetUserFromDatabaseWithUsername(String username)
        {
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                List<User> users = conn.Query<User>("SELECT userID, userName, emailAddress, password, apiKey, userRole, accountCreated FROM Users where userName in @usernames", new { usernames = new[] { username } }).ToList();
                if (users.Count != 1)
                {
                    return (User)null;
                } else
                {
                    return users[0];
                }
            }
        }

        public int InsertUser(User user)
        {
            int affectedRows = -1;
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    string sqlCommand = "INSERT INTO Users (userName, emailAddress, password, ApiKey, userRole)";
                    sqlCommand += " VALUES (@UserName, @EmailAddress, @Password, @ApiKey, @UserRole)";
                    affectedRows = conn.Execute(sqlCommand, user);
                } catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString()); // TODO change exception handling
                }
            }
            return affectedRows;
        }

        public int UpdateUser(User user)
        {
            int affectedRows = -1;
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    string sqlCommand = @"
                        UPDATE Users SET userName = @UserName
                        , emailAddress = @EmailAddress
                        , password = @Password
                        , apiKey = @ApiKey
                        , userRole = @UserRole
                        WHERE userID = @UserID";
                    affectedRows = conn.Execute(sqlCommand, user);
                } catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString()); // TODO change exception handling
                }
            }
            return affectedRows;
        }

        public int DeleteByName(string UserName)
        {
            int affectedRows = -1;
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    affectedRows = conn.Execute(@"DELETE FROM Users WHERE userName = @name", new { name = new[] { UserName } });
                } catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString()); // TODO change exception handling
                }
            }
            return affectedRows;
        }
    }
}
