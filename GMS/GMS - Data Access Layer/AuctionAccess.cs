using Dapper;
using GMS___Model;
using NLog;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GMS___Data_Access_Layer
{
    public class AuctionAccess : AuctionAccessIF
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        public int InsertAuction(Auction auction)
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
                    log.Info("Inserting auction to database with creator ID : @creatorID",auction.CreatorID);
                    affectedRows = conn.Execute("INSERT INTO Auction (creatorID, eventID, itemID) VALUES (@CreatorID, @EventID, @ItemID)", auction);
                    log.Info("Successfully inserted auction to database.");
                } catch (SqlException ex)
                {
                    log.Trace("SQLException while inserting auction to database.");
                    log.Error(ex,"Unable to insert auction to database.");
                }
            }
            return affectedRows;
        }
        public Auction GetAuctionFromDatabase(int auctionID)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new Exception(), "No connection to either the internet or the database available.");
                return null;
            }

            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Retrieving auction from the database with auction ID: @auctionID", auctionID);
                    if (conn.QueryFirst<Auction>("SELECT * FROM Auction WHERE auctionID in @AuctionID", new { AuctionID = auctionID }) is Auction auction)
                    {
                        log.Info("Sucessfully retrieved auction from database.");
                        return auction;
                    } else
                    {
                        log.Error(new Exception(), "Unable to retrieve auction from database.");
                        return null;
                        
                    }
                } catch(SqlException ex)
                {
                    log.Trace("SQLException encountered while retrieving the auction from the database.");
                    log.Error(ex, "Unable to retrieve auction from database.");
                    return null;
                }
            }
        }
        public int DeleteByCreatorID(int CreatorID)
        {

            if (!DBConnection.IsConnectionAvailable())
            {
                log.Error(exception: new Exception(), "No connection to either the internet or the database available.");
                return -1;
            }

            int affectedRows = -1;
            using (IDbConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    log.Info("Deleting auction from database with creator ID: @creatorID", CreatorID);
                    affectedRows = conn.Execute(@"DELETE FROM Auction WHERE creatorID = @id", new { id = CreatorID });
                    log.Info("Successfully deleted auction from the database.");
                } catch (SqlException ex)
                {
                    log.Trace("SQLException encountered while deleting the auction from the database.");
                    log.Error(ex, "Unable to delete auction from database.");
                }
            }
            return affectedRows;
        }
    }
}
