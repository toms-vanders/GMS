using System;
using System.Collections.Generic;
using System.Text;
using GMS___Model;

namespace GMS___Data_Access_Layer
{
    interface AuctionAccessIF
    {
        int InsertAuction(Auction action);
        Auction GetAuctionFromDatabase(int auctionID);
    }
}
