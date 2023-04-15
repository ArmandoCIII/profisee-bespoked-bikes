using Amazon.DynamoDBv2.DataModel;
using SalesPersonManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SalesPersonManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesPersonController : ControllerBase
    {
        private readonly IDynamoDBContext _dynamoDBContext;
        public SalesPersonController(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSalesPerson()
        {
            var salesperson = await _dynamoDBContext.ScanAsync<SalesPerson>(default).GetRemainingAsync();
            return Ok(salesperson);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSalesPerson(SalesPerson newSalesPerson)
        {
            //TODO: Careful to add LastName for sort key
            var salesperson = await _dynamoDBContext.LoadAsync<SalesPerson>(newSalesPerson.FirstName);
            if (salesperson != null) return BadRequest($"salesperson Already Exists");
            await _dynamoDBContext.SaveAsync(salesperson);
            return Ok(salesperson);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSalesPerson(SalesPerson newSalesPerson)
        {
        //TODO: Careful to add LastName for sort key
            var salesperson = await _dynamoDBContext.LoadAsync<SalesPerson>(newSalesPerson.FirstName);
            if (salesperson == null) return BadRequest($"salesperson Does Not Exist");
            await _dynamoDBContext.SaveAsync(salesperson);
            return Ok(salesperson);
        }
    }
}
