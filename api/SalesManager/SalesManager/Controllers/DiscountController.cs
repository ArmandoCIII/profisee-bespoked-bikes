using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesManager.Models;

namespace SalesManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDynamoDBContext _dynamoDBContext;
        public DiscountController(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDiscount()
        {
            var discount = await _dynamoDBContext.ScanAsync<Discount>(default).GetRemainingAsync();
            return Ok(discount);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDiscount(Discount newDiscount)
        {
            //TODO: Careful to add LastName for sort key
            var discount = await _dynamoDBContext.LoadAsync<Discount>(newDiscount.Product);
            if (discount != null) return BadRequest($"Discount Already Exists");
            await _dynamoDBContext.SaveAsync(discount);
            return Ok(discount);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateDiscount(Discount newDiscount)
        {
            //TODO: Careful to add LastName for sort key
            var discount = await _dynamoDBContext.LoadAsync<Discount>(newDiscount.Product);
            if (discount == null) return BadRequest($"Discount Does Not Exist");
            await _dynamoDBContext.SaveAsync(discount);
            return Ok(discount);
        }
    }
}
