using EAuctionBuyer.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuctionBuyer.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductByIDAsync(string ProductID);
        Task<Product> CreateOrUpdateAsync(Product product);
        Task<bool> DeleteAsync(string ProductID);
    }
}
