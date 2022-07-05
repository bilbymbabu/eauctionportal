using EAuctionSeller.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuctionSeller.Repository
{
    public interface IProductToBuyerRepository
    {
        Task<List<ProductToBuyer>> GetProductsByBuyerIdAsync();
        Task<List<ProductToBuyer>> GetBidByProductIDAsync(string productId);
        Task<ProductToBuyer> GetProductByUserIDAsync(string productId,string UserID);
        Task<ProductToBuyer> CreateOrUpdateAsync(ProductToBuyer product);
        Task<bool> DeleteAsync(string ProductID, string userID);
    }
}
