using EAuctionSeller.Data;
using EAuctionSeller.Enum;
using EAuctionSeller.Extensions;
using EAuctionSeller.Helpers;
using EAuctionSeller.Interface;
using EAuctionSeller.MessageBroker;
using EAuctionSeller.Model;
using EAuctionSeller.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuctionSeller.Services
{
    public class ProductService:IProduct
    {
        public readonly IProductRepository productRepository;
        public readonly IProductToBuyerRepository productToBuyerRepository;
        public readonly IUserRepository userRepository;
        private readonly IBidUpdateSender bidUpdateSender;
        public ProductService(IProductRepository _productRepository, IProductToBuyerRepository _productToBuyerRepository, IUserRepository _userRepository, IBidUpdateSender _bidUpdateSender)
        {
            productRepository = _productRepository;
            productToBuyerRepository=_productToBuyerRepository;
            userRepository = _userRepository;
            bidUpdateSender = _bidUpdateSender;
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
            
            return prod;
        }

        public async Task<Product> CreateOrUpdateProductAsync(Product product)
        {
            if (!ArgumentValidations.ValidateProductName(product.ProductName))
                throw new UserException("Please enter valid product name");
            if (product.ProductId != null)
            {
                var existingProduct = await productRepository.GetProductByIDAsync(product.ProductId);
                if (existingProduct == null)
                    throw new ProductNotFounException("Product not found");
                var updatedproduct = await productRepository.CreateOrUpdateAsync(product);
                return updatedproduct;
            }
            else
            {
                if (product.BidEndDate <= DateTime.Today)
                    throw new ProductException("Bid date should be in future");
                var newProduct = await productRepository.CreateOrUpdateAsync(product);
                bidUpdateSender.SendBid(product);
                return newProduct;
            }
           
        }

        public async Task<bool> DeleteAsync(string ProductID)
        {
            var existingProduct = await productRepository.GetProductByIDAsync(ProductID);
            if (existingProduct == null)
                throw new ProductNotFounException("Product not found");
            var bids= await productToBuyerRepository.GetBidByProductIDAsync(ProductID);
            if(bids.Count()>0)
                throw new ProductException("cannot delete the product");
            foreach (var item in bids)
            {
                if (item.CreatedDate > existingProduct.BidEndDate)
                    throw new ProductException("cannot delete the product");
            }
            return await productRepository.DeleteAsync(ProductID);
        }


        public async Task<List<Product>> GetProductsAsync()
        {
            var products= await productRepository.GetProductsAsync();
            return products;
        }

        public async Task<ProductBids> GetbidsByProductID(string ProductID)
        {
            var prod= await productRepository.GetProductByIDAsync(ProductID);
            var bids= await productToBuyerRepository.GetBidByProductIDAsync(ProductID);
            if (bids == null)
                throw new ProductNotFounException("Product not found");
            var category = CategoryEnum.Sculptor.ToString();
            switch (Convert.ToInt32(prod.Category))
            {
                case (int)CategoryEnum.Ornament:
                    {
                        category = CategoryEnum.Ornament.ToString();
                        break;
                    }
                case (int)CategoryEnum.Sculptor:
                    {
                        category = CategoryEnum.Sculptor.ToString();
                        break;
                    }
                case (int)CategoryEnum.Painting:
                    {
                        category = CategoryEnum.Painting.ToString();
                        break;
                    }
                default:
                    category = string.Empty;
                    break;

            }
            var productbids = new ProductBids()
            {
                ProductName=prod.ProductName,
                ShortDescription = prod.ShortDeceription,
                DetailedDeceription = prod.DetailedDeceription,
                Category = category,
                BidEndDate = prod.BidEndDate,
                StartingPrice = prod.StartingPrice
            };

            var bidDetails = new List<BidDetails>();
            foreach (var item in bids)
            {
               
                var existingUser = await userRepository.GetUserByIDAsync(item.UserID);
                var prodbid = new BidDetails()
                {
                    BidAmount=item.BidAmount,
                    BuyerName=existingUser.FirstName+" " + existingUser.LastName,
                    EmailId=existingUser.Email,
                    Phone=existingUser.Phone

                };
                bidDetails.Add(prodbid);
            }
            productbids.BidDetails = bidDetails.OrderByDescending(x=>x.BidAmount).ToList();
            
            return productbids;
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
