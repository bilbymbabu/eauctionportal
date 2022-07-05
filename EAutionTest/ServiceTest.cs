
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using IProductRepository = EAuctionBuyer.Repository.IProductRepository;
using IProductRepositorySeller = EAuctionSeller.Repository.IProductRepository;
using IProductToBuyerRepository = EAuctionBuyer.Repository.IProductToBuyerRepository;
using IProductToBuyerRepositorySeller = EAuctionSeller.Repository.IProductToBuyerRepository;
using IUserRepository = EAuctionBuyer.Repository.IUserRepository;
using IUserRepositorySeller = EAuctionSeller.Repository.IUserRepository;
using Product = EAuctionSeller.Data.Product;
using IRabbitMqListenerSeller = EAuctionSeller.MessageBroker.IRabbitMqProducer;
using EAuctionSeller.Model;

namespace EAutionTest
{
    public class ServiceTest
    {
        public Mock<IUserRepository> userRepository;
        public Mock<IUserRepositorySeller> userRepositorySeller;
        public  Mock<IProductRepository> productRepository;
        public Mock<IProductRepositorySeller> productRepositorySeller;
        public  Mock<IProductToBuyerRepository> productToBuyerRepository;
        public Mock<IProductToBuyerRepositorySeller> productToBuyerRepositorySeller;
        public Mock<IRabbitMqListenerSeller> rabbitmq;
        private EAuctionSeller.Data.User _user;
        private EAuctionBuyer.Data.User _userBuyer;
        private EAuctionSeller.Data.Product product;
        private BuyerBid buyerBid;
        private EAuctionSeller.Data.ProductToBuyer productToBuyer;

        [SetUp]
        public void Setup()
        {
            userRepositorySeller = new Mock<IUserRepositorySeller>();
            productRepositorySeller = new Mock<IProductRepositorySeller>();
            productToBuyerRepositorySeller = new Mock<IProductToBuyerRepositorySeller>();
            rabbitmq = new Mock<IRabbitMqListenerSeller>();
            productToBuyer = new EAuctionSeller.Data.ProductToBuyer() { ProductId = "PHSDNSDJCSDCDSHCH5678y3KCDKWD", BidAmount = 10, UserID = "GHJSDNSDJCSDCDSHCH5678y3KCDKWD" };
            _user = new EAuctionSeller.Data.User() {UserId= "GHJSDNSDJCSDCDSHCH5678y3KCDKWD", FirstName = "annaa", LastName = "sonna", Address = "abc", City = "city", State = "state" ,Phone="8086309015",Email="annasonna@gmail.com"};
            product = new Product() { ProductId="PHSDNSDJCSDCDSHCH5678y3KCDKWD", ProductName = "earrings", Category = "1", DetailedDeceription = "beautiful" };
            buyerBid = new BuyerBid() {ProductId= "PHSDNSDJCSDCDSHCH5678y3KCDKWD", FirstName = "annaa", LastName = "sonna", Address = "abc", City = "city", State = "state", BidAmount = 10 };
        }

        [Test]
       
        public void AddNewSeller_InvalidFirstName()
        {
            var _users = new EAuctionSeller.Data.User() { FirstName = "ann", LastName = "sonna", Address = "abc", City = "city", State = "state" };
            userRepositorySeller.Setup(x => x.GetUserByIDAsync("GHJSDNSDJCSDCDSHCH5678y3KCDKWD")).ReturnsAsync(_users);
            userRepositorySeller.Setup(x => x.CreateOrUpdateAsync(_users)).ReturnsAsync(_users);
            var service = new EAuctionSeller.Services.UserService(userRepositorySeller.Object);
            var result =  service.CreateOrUpdateAsync(_users);
            Assert.IsInstanceOf<AggregateException>(result.Exception);

        }

        [Test]

        public void AddNewSeller_InvalidLastName()
        {
            var _users = new EAuctionSeller.Data.User() { FirstName = "annss", LastName = "s", Address = "abc", City = "city", State = "state" };
            userRepositorySeller.Setup(x => x.GetUserByIDAsync("GHJSDNSDJCSDCDSHCH5678y3KCDKWD")).ReturnsAsync(_users);
            userRepositorySeller.Setup(x => x.CreateOrUpdateAsync(_users)).ReturnsAsync(_users);
            var service = new EAuctionSeller.Services.UserService(userRepositorySeller.Object);
            var result = service.CreateOrUpdateAsync(_users);
            Assert.IsInstanceOf<AggregateException>(result.Exception);

        }
        [Test]

        public void AddNewSeller_InvalidEmailID()
        {
            var _users = new EAuctionSeller.Data.User() { FirstName = "annee", LastName = "sonna", Address = "abc", City = "city", State = "state",Email="asd#gmail.com" };
            userRepositorySeller.Setup(x => x.GetUserByIDAsync("GHJSDNSDJCSDCDSHCH5678y3KCDKWD")).ReturnsAsync(_users);
            userRepositorySeller.Setup(x => x.CreateOrUpdateAsync(_users)).ReturnsAsync(_users);
            var service = new EAuctionSeller.Services.UserService(userRepositorySeller.Object);
            var result = service.CreateOrUpdateAsync(_users);
            Assert.IsInstanceOf<AggregateException>(result.Exception);

        }
        [Test]

        public void AddNewSeller_InvalidPhone()
        {
            var _users = new EAuctionSeller.Data.User() { FirstName = "ann", LastName = "sonna", Address = "abc", City = "city", State = "state",Phone="345#@$@$" };
            userRepositorySeller.Setup(x => x.GetUserByIDAsync("GHJSDNSDJCSDCDSHCH5678y3KCDKWD")).ReturnsAsync(_users);
            userRepositorySeller.Setup(x => x.CreateOrUpdateAsync(_users)).ReturnsAsync(_users);
            var service = new EAuctionSeller.Services.UserService(userRepositorySeller.Object);
            var result = service.CreateOrUpdateAsync(_users);
            Assert.IsInstanceOf<AggregateException>(result.Exception);

        }

        [Test]

        public void AddNewSeller_200Ok()
        {
            userRepositorySeller.Setup(x => x.GetUserByIDAsync("GHJSDNSDJCSDCDSHCH5678y3KCDKWD")).ReturnsAsync(_user);
            userRepositorySeller.Setup(x => x.CreateOrUpdateAsync(_user)).ReturnsAsync(_user);
            var service = new EAuctionSeller.Services.UserService(userRepositorySeller.Object);
            var result = service.CreateOrUpdateAsync(_user);
            Assert.IsNotNull(result.Result);
            Assert.AreEqual("GHJSDNSDJCSDCDSHCH5678y3KCDKWD", result.Result.UserId);

        }

        [Test]

        public void AddNewSeller_InvalidUser()
        {
            userRepositorySeller.Setup(x => x.GetUserByIDAsync("GHJSDNSDJCSDCDSHCH5678")).ReturnsAsync(_user);
            userRepositorySeller.Setup(x => x.CreateOrUpdateAsync(_user)).ReturnsAsync(_user);
            var service = new EAuctionSeller.Services.UserService(userRepositorySeller.Object);
            var result = service.CreateOrUpdateAsync(_user);
            Assert.AreEqual("User not found",result.Exception.InnerException.Message);
        }

        [Test]

        public void AddBidForProduct_InvalidPRoduct()
        {
            productRepositorySeller.Setup(x => x.GetProductByIDAsync("GHJSDNSDJCSDCDSHCH5678")).ReturnsAsync(product);
            userRepositorySeller.Setup(x => x.CreateOrUpdateAsync(_user)).ReturnsAsync(_user);
            productToBuyerRepositorySeller.Setup(x => x.CreateOrUpdateAsync(productToBuyer)).ReturnsAsync(productToBuyer);
            var userservice = new EAuctionSeller.Services.UserService(userRepositorySeller.Object);
            var service = new EAuctionSeller.Services.ProductService(productRepositorySeller.Object,productToBuyerRepositorySeller.Object, userRepositorySeller.Object,rabbitmq.Object);
            var bidResult = service.AddBidForProductAsync(buyerBid);
            Assert.AreEqual("Product not found", bidResult.Exception.InnerException.Message);
        }

        [Test]

        public void AddBidForProduct_InvalidBiddate()
        {
          var _product = new Product() { ProductId = "PHSDNSDJCSDCDSHCH5678y3KCDKWD", ProductName = "earrings", Category = "1", DetailedDeceription = "beautiful",BidEndDate=DateTime.Today };

            productRepositorySeller.Setup(x => x.GetProductByIDAsync("PHSDNSDJCSDCDSHCH5678y3KCDKWD")).ReturnsAsync(_product);
            userRepositorySeller.Setup(x => x.CreateOrUpdateAsync(_user)).ReturnsAsync(_user);
            productToBuyerRepositorySeller.Setup(x => x.CreateOrUpdateAsync(productToBuyer)).ReturnsAsync(productToBuyer);
            var service = new EAuctionSeller.Services.ProductService(productRepositorySeller.Object, productToBuyerRepositorySeller.Object, userRepositorySeller.Object, rabbitmq.Object);
            
            var bidResult = service.AddBidForProductAsync(buyerBid);
            Assert.AreEqual("Cannot place this bid", bidResult.Exception.InnerException.Message);
        }

        [Test]

        public void AddBidForProduct_200Ok()
        { 
            productRepositorySeller.Setup(x => x.GetProductByIDAsync("PHSDNSDJCSDCDSHCH5678y3KCDKWD")).ReturnsAsync(product);
            userRepositorySeller.Setup(x => x.CreateOrUpdateAsync(_user)).ReturnsAsync(_user);
            productToBuyerRepositorySeller.Setup(x => x.CreateOrUpdateAsync(productToBuyer)).ReturnsAsync(productToBuyer);
            var service = new EAuctionSeller.Services.ProductService(productRepositorySeller.Object, productToBuyerRepositorySeller.Object, userRepositorySeller.Object, rabbitmq.Object);

            var result = service.AddBidForProductAsync(buyerBid);
            Assert.IsNotNull(result);
        }

        [Test]

        public void AddProduct_200Ok()
        {
            productRepositorySeller.Setup(x => x.GetProductByIDAsync("PHSDNSDJCSDCDSHCH5678y3KCDKWD")).ReturnsAsync(product);
            productToBuyerRepositorySeller.Setup(x => x.CreateOrUpdateAsync(productToBuyer)).ReturnsAsync(productToBuyer);
           var service = new EAuctionSeller.Services.ProductService(productRepositorySeller.Object, productToBuyerRepositorySeller.Object, userRepositorySeller.Object, rabbitmq.Object);

            var result = service.CreateOrUpdateProductAsync(product);
            Assert.IsNotNull(result);
        }

        [Test]

        public void AddProduct_InvalidPRoduct()
        {
            productRepositorySeller.Setup(x => x.GetProductByIDAsync("GHJSDNSDJCSDCDSHCH5678")).ReturnsAsync(product);

            productRepositorySeller.Setup(x => x.CreateOrUpdateAsync(product)).ReturnsAsync(product);
            
            var service = new EAuctionSeller.Services.ProductService(productRepositorySeller.Object, productToBuyerRepositorySeller.Object, userRepositorySeller.Object, rabbitmq.Object);
            var result = service.CreateOrUpdateProductAsync(product);
            Assert.AreEqual("Product not found", result.Exception.InnerException.Message);
        }

        [Test]

        public void AddProduct_InvalidPRoductName()
        {
            var _product = new Product() { ProductId = "PHSDNSDJCSDCDSHCH5678y3KCDKWD", ProductName = "ea", Category = "1", DetailedDeceription = "beautiful" };

            productRepositorySeller.Setup(x => x.GetProductByIDAsync("PHSDNSDJCSDCDSHCH5678y3KCDKWD")).ReturnsAsync(product);

            productRepositorySeller.Setup(x => x.CreateOrUpdateAsync(_product)).ReturnsAsync(_product);

            var service = new EAuctionSeller.Services.ProductService(productRepositorySeller.Object, productToBuyerRepositorySeller.Object, userRepositorySeller.Object, rabbitmq.Object);
            var result = service.CreateOrUpdateProductAsync(_product);
            Assert.AreEqual("Please enter valid product name", result.Exception.InnerException.Message);
        }

        [Test]

        public void AddProduct_InvalidPRoductBidEnDate()
        {
            var _product = new Product() {  ProductName = "earrings", Category = "1", DetailedDeceription = "beautiful",BidEndDate=DateTime.Today };

            productRepositorySeller.Setup(x => x.GetProductByIDAsync("PHSDNSDJCSDCDSHCH5678y3KCDKWD")).ReturnsAsync(product);

            productRepositorySeller.Setup(x => x.CreateOrUpdateAsync(_product)).ReturnsAsync(_product);

            var service = new EAuctionSeller.Services.ProductService(productRepositorySeller.Object, productToBuyerRepositorySeller.Object, userRepositorySeller.Object, rabbitmq.Object);
            var result = service.CreateOrUpdateProductAsync(_product);
            Assert.AreEqual("Bid date should be in future", result.Exception.InnerException.Message);
        }

        [Test]

        public void DeleteProduct_InvalidPRoduct()
        {
            productRepositorySeller.Setup(x => x.GetProductByIDAsync("GHJSDNSDJCSDCDSHCH5678")).ReturnsAsync(product);
            
            var service = new EAuctionSeller.Services.ProductService(productRepositorySeller.Object, productToBuyerRepositorySeller.Object, userRepositorySeller.Object, rabbitmq.Object);
            var result = service.DeleteAsync(product.ProductId);
            Assert.AreEqual("Product not found", result.Exception.InnerException.Message);
        }

        [Test]

        public void DeleteProduct_ProductWithBid()
        {
            List<EAuctionSeller.Data.ProductToBuyer> productToBuyers = new List<EAuctionSeller.Data.ProductToBuyer>();
            productToBuyers.Add(productToBuyer);
            productRepositorySeller.Setup(x => x.GetProductByIDAsync(product.ProductId)).ReturnsAsync(product);
            productToBuyerRepositorySeller.Setup(x => x.GetBidByProductIDAsync(product.ProductId)).ReturnsAsync(productToBuyers);
            var service = new EAuctionSeller.Services.ProductService(productRepositorySeller.Object, productToBuyerRepositorySeller.Object, userRepositorySeller.Object, rabbitmq.Object);
            var result = service.DeleteAsync(product.ProductId);
            Assert.AreEqual("cannot delete the product", result.Exception.InnerException.Message);
        }

        [Test]

        public void DeleteProduct_200Ok()
        {
            List<EAuctionSeller.Data.ProductToBuyer> productToBuyers = new List<EAuctionSeller.Data.ProductToBuyer>();
            var _productToBuyer = new EAuctionSeller.Data.ProductToBuyer() { ProductId = "PHSDNSDJCSDCDSHCH5678y3KCD", BidAmount = 10, UserID = "GHJSDNSDJCSDCDSHCH5678y3KCDKWD" };
            productToBuyers.Add(_productToBuyer);
            productRepositorySeller.Setup(x => x.GetProductByIDAsync(product.ProductId)).ReturnsAsync(product);
            productToBuyerRepositorySeller.Setup(x => x.GetBidByProductIDAsync(product.ProductId)).ReturnsAsync(productToBuyers);
            var service = new EAuctionSeller.Services.ProductService(productRepositorySeller.Object, productToBuyerRepositorySeller.Object, userRepositorySeller.Object, rabbitmq.Object);
            var result = service.DeleteAsync(product.ProductId);
            Assert.IsNotNull(result);
        }

        [Test]

        public void GetBidsByProductId_200Ok()
        {
            List<EAuctionSeller.Data.ProductToBuyer> productToBuyers = new List<EAuctionSeller.Data.ProductToBuyer>();
            productToBuyers.Add(productToBuyer);
            productRepositorySeller.Setup(x => x.GetProductByIDAsync(product.ProductId)).ReturnsAsync(product);
            productToBuyerRepositorySeller.Setup(x => x.GetBidByProductIDAsync(product.ProductId)).ReturnsAsync(productToBuyers);
            var service = new EAuctionSeller.Services.ProductService(productRepositorySeller.Object, productToBuyerRepositorySeller.Object, userRepositorySeller.Object, rabbitmq.Object);
            var bidResult = service.GetbidsByProductID(product.ProductId);
            Assert.IsNotNull(bidResult);
        }

        [Test]

        public void GetBidsByProductId_InvalidProduct()
        {
            List<EAuctionSeller.Data.ProductToBuyer> productToBuyers = new List<EAuctionSeller.Data.ProductToBuyer>();
            productToBuyers.Add(productToBuyer);
            productRepositorySeller.Setup(x => x.GetProductByIDAsync(product.ProductId)).ReturnsAsync(product);
            productToBuyerRepositorySeller.Setup(x => x.GetBidByProductIDAsync(product.ProductId)).ReturnsAsync(productToBuyers);
            var service = new EAuctionSeller.Services.ProductService(productRepositorySeller.Object, productToBuyerRepositorySeller.Object, userRepositorySeller.Object, rabbitmq.Object);
            var bidResult = service.GetbidsByProductID("DSFSFa");
            Assert.AreEqual("Product not found", bidResult.Exception.InnerException.Message);
        }

        [Test]

        public void GetBidsByProductId_ThrowsException()
        {
            List<EAuctionSeller.Data.ProductToBuyer> productToBuyers = new List<EAuctionSeller.Data.ProductToBuyer>();
            productToBuyers.Add(productToBuyer);
            productRepositorySeller.Setup(x => x.GetProductByIDAsync(product.ProductId)).ReturnsAsync(product);
            productToBuyerRepositorySeller.Setup(x => x.GetBidByProductIDAsync(product.ProductId)).Throws(new Exception());
            var service = new EAuctionSeller.Services.ProductService(productRepositorySeller.Object, productToBuyerRepositorySeller.Object, userRepositorySeller.Object, rabbitmq.Object);
            var Result = service.GetbidsByProductID("DSFSFa");
            Assert.IsInstanceOf<AggregateException>(Result.Exception);
        }

       
    }
}
