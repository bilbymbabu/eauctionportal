using EAuctionSeller.Data;
using System.Threading.Tasks;

namespace EAuctionSeller.Interface
{
    public interface IUser
    {
        //Task<List<User>> GetUSersAsync();
        //Task<User> GetUserByIDAsync(string UserId);
        Task<User> CreateOrUpdateAsync(User user);
        //Task<bool> DeleteAsync(string UserId);
    }
}
