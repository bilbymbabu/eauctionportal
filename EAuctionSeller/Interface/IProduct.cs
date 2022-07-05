using EAuctionSeller.Data;
using EAuctionSeller.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuctionSeller.Interface
{
    public interface IProduct
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> CreateOrUpdateProductAsync(Product product);
        Task<bool> DeleteAsync(string ProductID);
        Task<ProductToBuyer> AddBidForProductAsync(BuyerBid product);
        Task<ProductBids> GetbidsByProductID(string ProductID);
        Task<ProductToBuyer> UpdateBidForProduct(string ProductID, string buyerEmail, decimal amount);
    }
}
