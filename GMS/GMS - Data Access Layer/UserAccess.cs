using Dapper;
using GMS___Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GMS___Data_Access_Layer
{
    public class UserAccess : IUserAccess
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        public IEnumerable<User> GetUsersFromDatabase()
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return null;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Retrieving all users from the database.");
                    IEnumerable<User> users = conn.Query<User>("SELECT * FROM Users");
                    log.Info("Successfully retrieved all users from the database");
                    return users;
                } catch (SqlException ex)
                {
                    log.Trace("SQLException while retrieving the users from the database.");
                    log.Error(ex, "Unable to retrieve the users from the database.");
                    return null;
                }
            }
        }

        public User GetUserFromDatabase(string emailAddress)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return null;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Retrieving the user with email: @email", emailAddress);
                    if (conn.QueryFirst<User>("SELECT userID, userName, emailAddress, password, apiKey, userRole, accountCreated FROM Users where emailAddress = @emails", new { emails = emailAddress }) is User user)
                    {
                        log.Info("Successfully retrieved the user with email: @email from the database", emailAddress);
                        return user;
                    } else
                    {
                        return null;
                    }
                } catch (SqlException ex)
                {
                    log.Trace("SQLException while retrieving the user from the database.");
                    log.Error(ex, "Unable to retrieve the user from the database.");
                    return null;
                }
            }
        }

        public User GetUserFromDatabaseWithUsername(string username)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return null;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Retrieving the user with username: @username", username);
                    if (conn.QueryFirst<User>("SELECT userID, userName, emailAddress, password, apiKey, userRole, accountCreated FROM Users WHERE userName = @usernames", new { usernames = username }) is User user)
                    {
                        log.Info("Successfully retrieved the user wtih username: @username", username);
                        return user;
                    } else
                    {
                        return null;
                    }
                } catch (SqlException ex)
                {
                    log.Trace("SQLException while retrieving the user from the database.");
                    log.Error(ex, "Unable to retrieve the user from the database.");
                    return null;
                }
            }
        }

        public int InsertUser(User user)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return -1;
            }

            int affectedRows = -1;
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Inserting user with username: @username into the database.", user.UserName);
                    affectedRows = conn.Execute("INSERT INTO Users (userName, emailAddress, password, ApiKey, userRole) VALUES (@UserName, @EmailAddress, @Password, @ApiKey, @UserRole)", user);
                    log.Info("Successfully inserted the user with username: @username into the database", user.UserName);
                } catch (SqlException ex)
                {
                    log.Trace("SQLException while inserting the user to the database.");
                    log.Error(ex, "Unable to insert the user to the database.");
                    return affectedRows;
                }
            }
            return affectedRows;
        }

        public int UpdateUser(User user)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return -1;
            }

            int affectedRows = -1;
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Updating the user: @username into the database.", user.UserName);
                    affectedRows = conn.Execute(@"UPDATE Users SET userName = @UserName
                        , emailAddress = @EmailAddress
                        , password = @Password
                        , apiKey = @ApiKey
                        , userRole = @UserRole
                        WHERE userID = @UserID", user);
                    log.Info("Successfully updated the user: @username into the database.", user.UserName);
                } catch (SqlException ex)
                {
                    log.Trace("SQLException while updating the user into database.");
                    log.Error(ex, "Unable to update the user into database.");
                    return affectedRows;
                }
            }
            return affectedRows;
        }

        public int DeleteByName(string UserName)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new TimeoutException(), "No connection to either the internet or the database available.");
                return -1;
            }

            int affectedRows = -1;
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Deleting the user: @username from the database,", UserName);
                    affectedRows = conn.Execute(@"DELETE FROM Users WHERE userName = @name", new { name = UserName });
                    log.Info("Successfully deleted the user: @username from the database.", UserName);
                } catch (SqlException ex)
                {
                    log.Trace("SQLException while deleting the user from the database.");
                    log.Error(ex, "Unable to delete the user from the database.");
                    return affectedRows;
                }
            }
            return affectedRows;
        }
    }
}
