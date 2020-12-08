using GMS___Data_Access_Layer;
using GMS___Model;
using System;

namespace GMS___Business_Layer
{
    class AuctionProcessor : AuctionProcessorIF
    {
        private AuctionAccess auctionAccess = new AuctionAccess();

        public Boolean InsertNewAuction(int creatorID, int eventID, int itemID)
        {
            Auction auctionToBeAdded = new Auction(creatorID, eventID, itemID);
            return auctionAccess.InsertAuction(auctionToBeAdded) == 1 ? true : false;
        }
    }
}
