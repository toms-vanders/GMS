using GMS___Model;

namespace GMS___Data_Access_Layer
{
    interface AuctionAccessIF
    {
        int InsertAuction(Auction action);
        Auction GetAuctionFromDatabase(int auctionID);
        int DeleteByCreatorID(int CreatorID);
    }
}
