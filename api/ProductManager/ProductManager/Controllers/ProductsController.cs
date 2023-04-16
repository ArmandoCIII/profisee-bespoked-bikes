using Amazon.DynamoDBv2.DataModel;
using ProductManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IDynamoDBContext _dynamoDBContext;
        public ProductsController(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }
        [HttpGet]
        [ActionName("get")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _dynamoDBContext.ScanAsync<Products>(default).GetRemainingAsync();
            return Ok(products);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Products newProduct)
        {
            var product = await _dynamoDBContext.LoadAsync<Products>(newProduct.Name);
            if (product != null) return BadRequest($"Product Already Exists");
            await _dynamoDBContext.SaveAsync(newProduct);
            return Ok(newProduct);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Products newProduct)
        {
            var product = await _dynamoDBContext.LoadAsync<Products>(newProduct.Name);
            if (product == null) return BadRequest($"Product Does Not Exist");
            await _dynamoDBContext.SaveAsync(newProduct);
            return Ok(newProduct);
        }
    }
}
