using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuctionBuyer.Model
{
    public class BidDetails
    {
        public decimal BidAmount { get; set; }
        public string BuyerName { get; set; }
        public string EmailId { get; set; }
        public string Phone { get; set; }
    }
}
