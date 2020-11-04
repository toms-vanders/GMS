using Dapper;
using GMS___Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


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

        public int InsertUser(User user)
        {
            int affectedRows = -1;
            using (IDbConnection conn = GetConnection())
            {
                try
                {
                    string sqlCommand = "INSERT INTO Users (userName, email, password, apiKey)";
                    sqlCommand += " VALUES (@userName, @email, @password, @apiKey)";
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
