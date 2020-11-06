using System;
using System.Collections.Generic;
using System.Text;

namespace GMS___Business_Layer
{
    interface AuctionProcessorIF
    {
        Boolean InsertNewAuction(int creatorID, int eventID, int itemID);
    }
}
