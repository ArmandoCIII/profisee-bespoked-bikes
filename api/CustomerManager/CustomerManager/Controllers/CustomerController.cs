using Amazon.DynamoDBv2.DataModel;
using CustomerManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IDynamoDBContext _dynamoDBContext;
        public CustomerController(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }
        [HttpGet]
        [ActionName("get")]
        public async Task<IActionResult> GetAllCustomer()
        {
            var customer = await _dynamoDBContext.ScanAsync<Customer>(default).GetRemainingAsync();
            return Ok(customer);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer newCustomer)
        {
            //TODO: Careful to add LastName for sort key
            var customer = await _dynamoDBContext.LoadAsync<Customer>(newCustomer.FirstName);
            if (customer != null) return BadRequest($"Customer Already Exists");
            await _dynamoDBContext.SaveAsync(newCustomer);
            return Ok(newCustomer);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(Customer newCustomer)
        {
            //TODO: Careful to add LastName for sort key
            var customer = await _dynamoDBContext.LoadAsync<Customer>(newCustomer.FirstName);
            if (customer == null) return BadRequest($"Customer Does Not Exist");
            await _dynamoDBContext.SaveAsync(newCustomer);
            return Ok(newCustomer);
        }
    }
}
