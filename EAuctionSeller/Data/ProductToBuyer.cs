using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EAuctionSeller.Data
{
    public class ProductToBuyer
    {
        [BsonId]       
        public string BuyerProductId { get; set; }
        public string ProductId { get; set; }
        public string UserID { get; set; }
        public decimal BidAmount { get; set; }       
        public DateTime CreatedDate { get; set; }

        public DateTime? Updateddate { get; set; }

    }
}
