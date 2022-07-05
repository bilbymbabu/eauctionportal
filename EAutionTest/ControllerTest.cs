using EAuctionBuyer.Controllers;
using EAuctionSeller.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using IProductBuyer = EAuctionBuyer.Interface.IProduct;
using IProduct = EAuctionSeller.Interface.IProduct;
using IUser = EAuctionSeller.Interface.IUser;
using ArgumentValidations = EAuctionSeller.Helpers.ArgumentValidations;

namespace EAutionTest
{
    public class ControllerTest
    {
        private Mock<IProduct> _productmock;
        private Mock<IProductBuyer> _productmockBuyer;
        private Mock<IUser> _usermock;
        private EAuctionSeller.Data.User _user;
        private EAuctionSeller.Data.Product product;
        private EAuctionBuyer.Model.BuyerBid buyerBid;
        private  Mock<ILogger<SellerController>> _logger;
        private  Mock<ILogger<BuyerController>> _buyerlogger;

        [SetUp]
        public void Setup()
        {
            _productmock = new Mock<IProduct>();
            _productmockBuyer = new Mock<IProductBuyer>();
            _usermock = new Mock<IUser>();
            _logger = new Mock<ILogger<SellerController>>();
            _buyerlogger = new Mock<ILogger<BuyerController>>();
            _user = new EAuctionSeller.Data.User() {FirstName="annaa",LastName="sonna",Address="abc",City="city",State="state" };
            product = new EAuctionSeller.Data.Product() {ProductName="earrings" , Category="1", DetailedDeceription="beautiful"};
            buyerBid = new EAuctionBuyer.Model.BuyerBid() { FirstName = "annaa", LastName = "sonna", Address = "abc", City = "city", State = "state", BidAmount = 10 };
        }

        [TestCase("an")]
        [TestCase("ann")]
        public void AddNewSeller_InvalidFirstName(string value)
        {
            var result = ArgumentValidations.ValidateFirstName(value);
            Assert.IsFalse(result);
        }

        
        [TestCase("mariarose")]
        public void AddNewSeller_ValidFirstName(string value)
        {
            var result = ArgumentValidations.ValidateFirstName(value);
            Assert.IsTrue(result);
        }
        [TestCase("ro")]
        public void AddNewSeller_InvalidLastName(string value)
        {
            var result = ArgumentValidations.ValidateLastName(value);
            Assert.IsFalse(result);
        }

        [TestCase("rose")]
        [TestCase("mariarose")]
        public void AddNewSeller_ValidLastName(string value)
        {
            var result = ArgumentValidations.ValidateLastName(value);
            Assert.IsTrue(result);
        }
        [TestCase("an")]
        [TestCase("ann")]
        public void AddNewSeller_InvalidProductName(string value)
        {
            var result = ArgumentValidations.ValidateProductName(value);
            Assert.IsFalse(result);
        }

        [TestCase("earrings")]
        public void AddNewSeller_IValidProductName(string value)
        {
            var result = ArgumentValidations.ValidateProductName(value);
            Assert.IsTrue(result);
        }
        [TestCase("anna#gmail.com")]
        [TestCase("anna@gmail")]
        [TestCase("anna@gmailcom")]
        public void IsNotValidEmail(string value)
        {
            bool isValid = ArgumentValidations.IsValidEmail(value);
            Assert.IsFalse(isValid);
        }

        [TestCase("anna@gmail.com")]
        public void IsValidEmail(string value)
        {
            bool isValid = ArgumentValidations.IsValidEmail(value);
            Assert.IsTrue(isValid);
        }

        [TestCase("sdgjadjhasdmkjd")]
        [TestCase("564738921")]
        public void IsNotValidPhoneNumber(string value)
        {
            bool isValid = ArgumentValidations.ValidatePhoneNumber(value);
            Assert.IsFalse(isValid);
        }

        [Test]
        public void AddNewSeller_200Ok()
        {
            _usermock.Setup(x => x.CreateOrUpdateAsync(_user)).ReturnsAsync(_user);
            var controller = new SellerController(_logger.Object,_productmock.Object, _usermock.Object);
            var actionResult = controller.AddNewSeller(_user);
            Assert.IsNotNull(actionResult);
        }

        [Test]
        public void AddNewSeller_ThrowsException()
        {
            _usermock.Setup(x => x.CreateOrUpdateAsync(_user)).Throws(new Exception());
            var controller = new SellerController(_logger.Object, _productmock.Object, _usermock.Object);
           var result=controller.AddNewSeller(_user);
            Assert.IsInstanceOf<AggregateException>(result.Exception);
            
        }

        [Test]
        public void AddNewProduct_200Ok()
        {
            _productmock.Setup(x => x.CreateOrUpdateProductAsync(product)).ReturnsAsync(product);
            var controller = new SellerController(_logger.Object, _productmock.Object, _usermock.Object);
            var actionResult = controller.AddProduct(product);
            Assert.IsNotNull(actionResult);
        }

        [Test]
        public void AddNewSProdut_ThrowsException()
        {
            _productmock.Setup(x => x.CreateOrUpdateProductAsync(product)).Throws(new Exception());
            var controller = new SellerController(_logger.Object, _productmock.Object, _usermock.Object);
            var result = controller.AddProduct(product);
            Assert.IsInstanceOf<AggregateException>(result.Exception);

        }

        [Test]
        public void AhowProducts_200Ok()
        {
            _productmock.Setup(x => x.GetProductsAsync()).ReturnsAsync(new List<EAuctionSeller.Data.Product>());
            var controller = new SellerController(_logger.Object, _productmock.Object, _usermock.Object);
            var actionResult = controller.ShowAllProducts();
            Assert.IsNotNull(actionResult);
        }

        [Test]
        public void AllBids_200Ok()
        {
            _productmock.Setup(x => x.GetbidsByProductID("DFG567WFREGE466ARWRG")).ReturnsAsync(new EAuctionSeller.Model.ProductBids());
            var controller = new SellerController(_logger.Object, _productmock.Object, _usermock.Object);
            var actionResult = controller.ShowBids("DFG567WFREGE466ARWRG");
            Assert.IsNotNull(actionResult);
        }

        [Test]
        public void placebid_200Ok()
        {
            _productmockBuyer.Setup(x => x.AddBidForProductAsync(buyerBid)).ReturnsAsync(new EAuctionBuyer.Data.ProductToBuyer());
            var controller = new BuyerController(_buyerlogger.Object, _productmockBuyer.Object);
            var actionResult = controller.PlaceBid(buyerBid);
            Assert.IsNotNull(actionResult);
        }

        [Test]
        public void PlaceBid_ThrowsException()
        {
            _productmockBuyer.Setup(x => x.AddBidForProductAsync(buyerBid)).Throws(new Exception());
            var controller = new BuyerController(_buyerlogger.Object, _productmockBuyer.Object );
            var result = controller.PlaceBid(buyerBid);
            Assert.IsInstanceOf<AggregateException>(result.Exception);

        }

        [Test]
        public void Updatebid_200Ok()
        {
            _productmockBuyer.Setup(x => x.UpdateBidForProduct("DFDFXVXV456y7DFGDFG","mariaroise@test.com",10)).ReturnsAsync(new EAuctionBuyer.Data.ProductToBuyer());
            var controller = new BuyerController(_buyerlogger.Object, _productmockBuyer.Object);
            var actionResult = controller.UpdateBid("DFDFXVXV456y7DFGDFG", "mariaroise@test.com", 10);
            Assert.IsNotNull(actionResult);
        }

        [Test]
        public void UpdateBid_ThrowsException()
        {
            _productmockBuyer.Setup(x => x.UpdateBidForProduct("DFDFXVXV456y7DFGDFG", "mariaroise@test.com", 10)).Throws(new Exception());
            var controller = new BuyerController(_buyerlogger.Object, _productmockBuyer.Object);
            var result = controller.UpdateBid("DFDFXVXV456y7DFGDFG", "mariaroise@test.com", 10);
            Assert.IsInstanceOf<AggregateException>(result.Exception);

        }
    }
}