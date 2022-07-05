using EAuctionBuyer.Data;
using EAuctionBuyer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuctionBuyer.Interface
{
    public interface IProduct
    {
        Task<ProductToBuyer> AddBidForProductAsync(BuyerBid product);      
        Task<ProductToBuyer> UpdateBidForProduct(string ProductID, string buyerEmail, decimal amount);
    }
}
