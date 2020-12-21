using System;

namespace GMS___Business_Layer
{
    interface IAuctionProcessor
    {
        Boolean InsertNewAuction(int creatorID, int eventID, int itemID);
    }
}
