
using EAuctionBuyer.Extensions;
using EAuctionBuyer.Interface;
using EAuctionBuyer.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EAuctionBuyer.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("e-auction/api/v{v:apiVersion}/[controller]")]
    public class BuyerController : ControllerBase
    {
        private readonly IProduct _product;
        private readonly ILogger<BuyerController> _logger;
        public BuyerController(ILogger<BuyerController> logger, IProduct product)
        {
            _product = product;
            _logger = logger;
        }

        [HttpPost]
        [Route("place-bid")]
        public async Task<IActionResult> PlaceBid([FromBody] BuyerBid product)
        {
            try
            {
                var result = await _product.AddBidForProductAsync(product);
                return Ok(result);

            }
            catch (ProductException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                return new BadRequestObjectResult(ex.Message.ToString());
            }
        }

        [HttpPost]
        [Route("update-bid/{ProductId}/{buyerEmailId}/{newBidAmount}")]
        public async Task<IActionResult> UpdateBid(string ProductId, string buyerEmailId, decimal newBidAmount)
        {
            try
            {
                var result = await _product.UpdateBidForProduct(ProductId, buyerEmailId, newBidAmount);
                return Ok(result);

            }
            catch (ProductException ex)
            {
                _logger.LogInformation($"Exception : {ex}");
                return new BadRequestObjectResult(ex.Message.ToString());
            }
        }
    }
}

