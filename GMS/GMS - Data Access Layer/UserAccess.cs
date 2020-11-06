using Dapper;
using GMS___Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GMS___Data_Access_Layer
{
    public class UserAccess
    {
        IDbConnection GetConnection()
        {
            return new SqlConnection("Server=hildur.ucn.dk;Database=dmaj0919_1081496;User Id=dmaj0919_1081496;Password=Password1!;");
        }

        public IEnumerable<User> GetUsersFromDatabase()
        {
            using (IDbConnection conn = GetConnection())
            {
                IEnumerable<User> users = conn.Query<User>("SELECT * FROM Users");
                return users;
            }
        }

        public User GetUserFromDatabase(String emailAddress)
        {
            using (IDbConnection conn = GetConnection())
            {
                List<User> users = conn.Query<User>("SELECT userID, userName, email, password, apiKey, userRole FROM Users where email in @emails", new { emails = new[] { emailAddress } }).ToList();
                if (users.Count != 1)
                {
                    return (User) null;
                } else
                {
                    return users[0];
                }
            }
        }

        public int InsertUser(User user)
        {
            int affectedRows = -1;
            using (IDbConnection conn = GetConnection())
            {
                try
                {
                    string sqlCommand = "INSERT INTO Users (userName, email, password, ApiKey, userRole)";
                    sqlCommand += " VALUES (@UserName, @EmailAddress, @Password, @ApiKey, @UserRole)";
                    affectedRows = conn.Execute(sqlCommand, user);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString()); // TODO change exception handling
                }
            }
            return affectedRows;
        }
    }

    

}
