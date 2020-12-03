using System;

namespace GMS___Business_Layer
{
    interface AuctionProcessorIF
    {
        Boolean InsertNewAuction(int creatorID, int eventID, int itemID);
    }
}
