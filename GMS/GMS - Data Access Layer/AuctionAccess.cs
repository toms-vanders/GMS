using Dapper;
using GMS___Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GMS___Data_Access_Layer
{
    public class AuctionAccess
    {
        IDbConnection GetConnection()
        {
            return new SqlConnection("Server=hildur.ucn.dk;Database=dmaj0919_1081496;User Id=dmaj0919_1081496;Password=Password1!;");
        }

        public int InsertAuction(Auction action)
        {
            int affectedRows = -1;
            using (IDbConnection conn = GetConnection())
            {
                try
                {
                    string sqlCommand = "INSERT INTO Auction (creatorID, eventID, itemID)";
                    sqlCommand += " VALUES (@CreatorID, @EventID, @ItemID)";
                    affectedRows = conn.Execute(sqlCommand, action);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString()); // TODO change exception handling
                }
            }
            return affectedRows;
        }
        public Auction GetAuctionFromDatabase(int auctionID)
        {
            using (IDbConnection conn = GetConnection())
            {
                // TODO this is not the best way to pass single parameter, not the best way to pass multiple parameters in a query
                List<Auction> auctions = conn.Query<Auction>("SELECT * FROM Auction WHERE auctionID in @ids", new { ids = new[] { auctionID } }).ToList();
                if (auctions.Count() != 1)
                {
                    return (Auction) null;
                } else
                {
                    return auctions[0];
                }
            }
        }
    }
}
