using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Http;
using SalesManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace SalesManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IDynamoDBContext _dynamoDBContext;
        public SalesController(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _dynamoDBContext.ScanAsync<Sales>(default).GetRemainingAsync();
            return Ok(sales);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSales(Sales newSales)
        {
            //TODO: Careful to add LastName for sort key
            var sales = await _dynamoDBContext.LoadAsync<Sales>(newSales.Product);
            if (sales != null) return BadRequest($"Sales Already Exists");
            await _dynamoDBContext.SaveAsync(sales);
            return Ok(sales);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSales(Sales newSales)
        {
            //TODO: Careful to add LastName for sort key
            var sales = await _dynamoDBContext.LoadAsync<Sales>(newSales.Product);
            if (sales == null) return BadRequest($"Sales Does Not Exist");
            await _dynamoDBContext.SaveAsync(sales);
            return Ok(sales);
        }
    }
}
