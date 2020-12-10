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

            if (!DBConnection.IsConnectionAvailable())
            {
                return null;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    IEnumerable<User> users = conn.Query<User>("SELECT * FROM Users");
                    return users;
                } catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }

        public User GetUserFromDatabase(string emailAddress)
        {
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    List<User> users = conn.Query<User>("SELECT userID, userName, emailAddress, password, apiKey, userRole, accountCreated FROM Users where emailAddress in @emails", new { emails = new[] { emailAddress } }).ToList();
                    if (users.Count != 1)
                    {
                        return null;
                    } else
                    {
                        return users[0];
                    }
                } catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }

        public User GetUserFromDatabaseWithUsername(string username)
        {
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    List<User> users = conn.Query<User>("SELECT userID, userName, emailAddress, password, apiKey, userRole, accountCreated FROM Users where userName in @usernames", new { usernames = new[] { username } }).ToList();
                    if (users.Count != 1)
                    {
                        return null;
                    } else
                    {
                        return users[0];
                    }
                } catch (Exception ex)
                {

                    throw ex;
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
                    affectedRows = conn.Execute("INSERT INTO Users (userName, emailAddress, password, ApiKey, userRole) VALUES (@UserName, @EmailAddress, @Password, @ApiKey, @UserRole)", user);
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
                    affectedRows = conn.Execute(@"UPDATE Users SET userName = @UserName
                        , emailAddress = @EmailAddress
                        , password = @Password
                        , apiKey = @ApiKey
                        , userRole = @UserRole
                        WHERE userID = @UserID", user);
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
