using GMS___Data_Access_Layer;
using GMS___Model;
using NLog;
using System;

namespace GMS___Business_Layer
{
    class AuctionProcessor : AuctionProcessorIF
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        private AuctionAccess auctionAccess = new AuctionAccess();

        public bool InsertNewAuction(int creatorID, int eventID, int itemID)
        {
            Auction auctionToBeAdded = new Auction(creatorID, eventID, itemID);
            return auctionAccess.InsertAuction(auctionToBeAdded) == 1;
        }
    }
}
