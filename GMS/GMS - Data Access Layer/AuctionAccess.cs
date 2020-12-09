using Dapper;
using GMS___Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GMS___Data_Access_Layer
{
    public class AuctionAccess : AuctionAccessIF
    {

        public int InsertAuction(Auction action)
        {
            int affectedRows = -1;
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    affectedRows = conn.Execute("INSERT INTO Auction (creatorID, eventID, itemID) VALUES (@CreatorID, @EventID, @ItemID)", action);
                } catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString()); // TODO change exception handling
                }
            }
            return affectedRows;
        }
        public Auction GetAuctionFromDatabase(int auctionID)
        {
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                // TODO this is not the best way to pass single parameter, not the best way to pass multiple parameters in a query
                List<Auction> auctions = conn.Query<Auction>("SELECT * FROM Auction WHERE auctionID in @ids", new { ids = new[] { auctionID } }).ToList();
                if (auctions.Count() != 1)
                {
                    return null;
                } else
                {
                    return auctions[0];
                }
            }
        }
        public int DeleteByCreatorID(int CreatorID)
        {
            int affectedRows = -1;
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    affectedRows = conn.Execute(@"DELETE FROM Auction WHERE creatorID = @id", new { id = new[] { CreatorID } });
                } catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString()); // TODO change exception handling
                }
            }
            return affectedRows;
        }
    }
}
