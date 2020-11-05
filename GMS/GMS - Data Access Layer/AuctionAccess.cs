using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace GMS___Data_Access_Layer
{
    class AuctionAccess
    {
        IDbConnection GetConnection()
        {
            return new SqlConnection("Server=hildur.ucn.dk;Database=dmaj0919_1081496;User Id=dmaj0919_1081496;Password=Password1!;");
        }

        /*public int InsertAuction(////)
        {
            int affectedRows = -1;
            using (IDbConnection conn = GetConnection())
            {
                try
                {
                    string sqlCommand = "INSERT INTO Auction (userName, email, password, ApiKey)";
                    sqlCommand += " VALUES (@UserName, @EmailAddress, @Password, @ApiKey)";
                    affectedRows = conn.Execute(sqlCommand, user);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString()); // TODO change exception handling
                }
            }
            return affectedRows;
        }*/
    }
}
