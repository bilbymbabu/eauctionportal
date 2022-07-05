using EAuctionBuyer.Data;
using EAuctionBuyer.Enum;
using EAuctionBuyer.Extensions;
using EAuctionBuyer.Helpers;
using EAuctionBuyer.Interface;
using EAuctionBuyer.MessageBroker;
using EAuctionBuyer.Model;
using EAuctionBuyer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuctionSeller.Services
{
    public class BuyerProductService:IProduct
    {
        public readonly IProductRepository productRepository;
        public readonly IProductToBuyerRepository productToBuyerRepository;
        public readonly IUserRepository userRepository;
        private readonly IBidUpdateSender _rabbitMqListener;
        public BuyerProductService(IProductRepository _productRepository, IProductToBuyerRepository _productToBuyerRepository, IUserRepository _userRepository, IBidUpdateSender rabbitMqListener)
        {
            productRepository = _productRepository;
            productToBuyerRepository=_productToBuyerRepository;
            userRepository = _userRepository;
            _rabbitMqListener = rabbitMqListener;
        }

        public async Task<ProductToBuyer> AddBidForProductAsync(BuyerBid product)
        {
            var existingProduct = await productRepository.GetProductByIDAsync(product.ProductId);
            if (existingProduct == null)
                throw new ProductNotFounException("Product not found");
            if (existingProduct.BidEndDate <= DateTime.Today)
                throw new ProductException("Cannot place this bid");
            var user = new User() {
                FirstName = product.FirstName,
                LastName = product.LastName,
                Address = product.Address,
                City = product.City,
                State = product.State,
                Pin = product.Pin,
                Phone = product.Phone,
                Email = product.Email,
                UserType="1"
            };
            var newuser=userRepository.CreateOrUpdateAsync(user);
            var pdoucttobuyer = new ProductToBuyer()
            {
                BidAmount = product.BidAmount,
                ProductId = product.ProductId,
                UserID = newuser.Result.UserId,
                CreatedDate=DateTime.Now
            };
            var prod = await productToBuyerRepository.CreateOrUpdateAsync(pdoucttobuyer);

            //Add message to RabbitMq
            _rabbitMqListener.SendBid(String.Format("New bid placed by {0} {1}", product.FirstName, product.LastName));
            return prod;
        }
        public async Task<ProductToBuyer> UpdateBidForProduct(string ProductID, string buyerEmail, decimal amount)
        {
            var user = await userRepository.GetUserByEmailAsync(buyerEmail);
            if(user==null)
                throw new UserNotFounException("User not found");
            var result= await productToBuyerRepository.GetProductByUserIDAsync(ProductID,user.UserId);
            result.BidAmount = amount;
            return await productToBuyerRepository.CreateOrUpdateAsync(result);
        }

       
    }
}
