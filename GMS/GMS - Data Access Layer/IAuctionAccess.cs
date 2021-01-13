using GMS___Model;

namespace GMS___Data_Access_Layer
{
    interface IAuctionAccess
    {
        int InsertAuction(Auction action);
        Auction GetAuctionFromDatabase(int auctionID);
        int DeleteByCreatorID(int CreatorID);
    }
}
