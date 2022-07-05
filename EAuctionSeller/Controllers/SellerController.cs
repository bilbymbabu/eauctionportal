using EAuctionSeller.Data;
using EAuctionSeller.Extensions;
using EAuctionSeller.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EAuctionSeller.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("e-auction/api/v{v:apiVersion}/[controller]")]
    public class SellerController : ControllerBase
    {        
        private readonly IProduct _product;
        private readonly IUser _user;
        private readonly ILogger<SellerController> _logger;
        public SellerController(ILogger<SellerController> ilogger, IProduct product, IUser user)
        {            
            _product = product;
            _user = user;
            _logger = ilogger;
        }

        
        [HttpPost]
        [Route("addSeller")]
        public async Task<IActionResult> AddNewSeller([FromBody] User user)
        {
            try
            {

                _logger.LogInformation($"Register New seller: {user.FirstName}"); 
                var result =await  _user.CreateOrUpdateAsync(user);
                return new OkObjectResult(result);

            }
            catch (ProductException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                return new BadRequestObjectResult(ex.Message.ToString());
            }
        }

        [HttpPost]
        [Route("addproduct")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            try
            {
                _logger.LogInformation($"Adding New product: {product.ProductName}");
                var result=await _product.CreateOrUpdateProductAsync(product);
                return new OkObjectResult(result);

            }
            catch (ProductException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                return new BadRequestObjectResult(ex.Message.ToString());
            }
        }

        [HttpGet]
        [Route("show-bids/{productID}")]
        public async Task<IActionResult> ShowBids(string productID)
        {
            try
            {
                _logger.LogInformation($"Showing all bids under product: {productID}");
                var result =await  _product.GetbidsByProductID(productID);
                return new OkObjectResult (result);

            }
            catch (ProductException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                return new BadRequestObjectResult(ex.Message.ToString());
            }
        }

        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> ShowAllProducts()
        {
            try
            {  
                var result = await _product.GetProductsAsync();
                return new OkObjectResult(result);

            }
            catch (ProductException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                return new BadRequestObjectResult(ex.Message.ToString());
            }
        }

        [HttpDelete]
        [Route("delete/{productID}")]
        public async Task<IActionResult> DeletProduct(string productID)
        {
            try
            {
                var result =await  _product.DeleteAsync(productID);
                return new OkObjectResult(result);

            }
            catch (ProductException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                return new BadRequestObjectResult(ex.Message.ToString());
            }
        }
    }
}
