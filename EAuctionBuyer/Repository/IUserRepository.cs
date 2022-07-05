using EAuctionBuyer.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuctionBuyer.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetUSersAsync();
        Task<User> GetUserByIDAsync(string UserId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateOrUpdateAsync(User user);
        Task<bool> DeleteAsync(string UserId);
    }
}
