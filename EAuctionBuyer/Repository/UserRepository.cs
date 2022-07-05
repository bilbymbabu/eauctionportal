using EAuctionBuyer.Data;
using EAuctionBuyer.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuctionBuyer.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly AuctionBuyerDBContext _dBContext;
        public UserRepository(AuctionBuyerDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public async Task<User> CreateOrUpdateAsync(User user)
        {
            try
            {

                if (user.UserId != null && user.UserId != string.Empty)
                 _dBContext._userCollection.ReplaceOne(x=>x.UserId == user.UserId,user);
                else
                { 
                    user.UserId = Guid.NewGuid().ToString();
                    await _dBContext._userCollection.InsertOneAsync(user);
                }

                var newUser= _dBContext._userCollection.Find(c => c.Email == user.Email).FirstOrDefaultAsync();
                return newUser.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(string UserId)
        {
            try
            {
                var user = await  _dBContext._userCollection.Find(c=>c.UserId == UserId).FirstOrDefaultAsync();
                if (user != null)
                {
                    await _dBContext._ProductToBuyerCollection.DeleteManyAsync(x => x.UserID == UserId);
                    await _dBContext._userCollection.DeleteManyAsync(x => x.UserId == UserId);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _dBContext._userCollection.Find(c => c.Email == email).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<User> GetUserByIDAsync(string userid)
        {
            try 
            { 
                var user = await _dBContext._userCollection.Find(c => c.UserId == userid).FirstOrDefaultAsync();
                return user;
            }
            catch(Exception ex)
            { throw ex; }
        }

        public async Task<List<User>> GetUSersAsync()
        {
           return await _dBContext._userCollection.Find(c => true).ToListAsync();
        }
    }
}
