using System;
using System.Collections.Generic;
using System.Text;

namespace GMS___Model
{
    public class Auction
    {
        public int AuctionID { get; set; }
        public int CreatorID { get; set; }
        public int EventID { get; set; }
        public DateTime DateAndTimeOfCreation { get; set; }
        public int ItemID { get; set; }
        public decimal CurrentPrice { get; set; }
        public int HighestBidderID { get; set; }

        // For inserting data to DB
        public Auction( int creatorID, int eventID, int itemID)
        {
            CreatorID = creatorID;
            EventID = eventID;
            ItemID = itemID;
            // Current price defaults to 0 in DB
            HighestBidderID = 0; 
        }

        // For fetching from DB
        public Auction(int auctionID, int creatorID, int eventID, DateTime dateAndTimeOfCreation, int itemID, decimal currentPrice, int highestBidderID)
        {
            CreatorID = creatorID;
            EventID = eventID;
            DateAndTimeOfCreation = dateAndTimeOfCreation;
            ItemID = itemID;
            CurrentPrice = currentPrice;
            HighestBidderID = highestBidderID;
        }
    }
}
