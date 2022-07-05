using EAuctionBuyer.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuctionBuyer.Data
{
    public class AuctionBuyerDBContext
    {
        public readonly IMongoCollection<User> _userCollection;
        public readonly IMongoCollection<Product> _ProductCollection;
        public readonly IMongoCollection<ProductToBuyer> _ProductToBuyerCollection;
        public AuctionBuyerDBContext(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _userCollection = database.GetCollection<User>(mongoDBSettings.Value.UserCollection);
            _ProductCollection = database.GetCollection<Product>(mongoDBSettings.Value.ProductCollection);
            _ProductToBuyerCollection = database.GetCollection<ProductToBuyer>(mongoDBSettings.Value.ProductToBuyerCollection);
        }
            
    }
}
