using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Http;
using SalesManager.Models;
using Microsoft.AspNetCore.Mvc;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;

namespace SalesManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IDynamoDBContext _dynamoDBContext;
        private readonly AmazonDynamoDBClient _dynamoDBClient;
        public SalesController(IDynamoDBContext dynamoDBContext, IOptions<AmazonDynamoDBConfig> options)
        {
            _dynamoDBContext = dynamoDBContext;
            _dynamoDBClient = new AmazonDynamoDBClient(options.Value);
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
            await _dynamoDBContext.SaveAsync(newSales);
            return Ok(newSales);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSales(Sales newSales)
        {
            //TODO: Careful to add LastName for sort key
            var sales = await _dynamoDBContext.LoadAsync<Sales>(newSales.Product);
            if (sales == null) return BadRequest($"Sales Does Not Exist");
            await _dynamoDBContext.SaveAsync(newSales);
            return Ok(newSales);
        }
        [HttpPost("dateRange")]
        public async Task<IActionResult> FilterDateRange([FromBody] DateRangeRequest body)
        {
            /*int startDayDate = body.StartDate.Day;
            int startMonthDate = body.StartDate.Month;
            int startYearDate = body.StartDate.Year;
            int endDayDate = body.EndDate.Day;
            int endMonthDate = body.EndDate.Month;
            int endYearDate = body.EndDate.Year;*/


            /*var startDate = new DateMap
            {
                Day = startDayDate,
                Month = startMonthDate,
                Year = startYearDate,
                Time = new TimeMap
                {
                    Hours = 00,
                    Minutes = 00,
                    Seconds = 00
                }
            };

            var endDate = new DateMap
            {
                Day = endDayDate,
                Month = endMonthDate,
                Year = endYearDate,
                Time = new TimeMap
                {
                    Hours = 23,
                    Minutes = 59,
                    Seconds = 59
                }
            };*/


            var startDate = body.StartDate;
            var endDate = body.EndDate;

            var request = new QueryRequest
            {
                TableName = "Sales",
                IndexName = "SalesDate",
                KeyConditionExpression = "SalesDate between :startDate and :endDate",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":startDate", new AttributeValue {S = startDate.ToString()}},
                    {":endDate", new AttributeValue {S = endDate.ToString()}}
                }

            };


            var response = await _dynamoDBClient.QueryAsync(request);
            var sales = response.Items.Select(item => _dynamoDBContext.FromDocument<Sales>(Document.FromAttributeMap(item)));
            return Ok(sales);
        }
        [HttpPost("SalesPerson")]
        public async Task<IActionResult> SalesPersonCommissionReport([FromBody] SalesPersonRequest body)
        {


            var firstName = body.FirstName;
            var lastName = body.LastName;

            var request = new QueryRequest
            {
                TableName = "Sales",
                IndexName = "SalesPerson",
                KeyConditionExpression = "SalesDate between :firstName and :lastName",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":startDate", new AttributeValue {S = firstName.ToString()}},
                    {":endDate", new AttributeValue {S = lastName.ToString()}}
                }

            };


            var response = await _dynamoDBClient.QueryAsync(request);
            var sales = response.Items.Select(item => _dynamoDBContext.FromDocument<Sales>(Document.FromAttributeMap(item)));
            return Ok(sales);
        }

        public class DateRangeRequest
        {
            public DateMap StartDate { get; set; }
            public DateMap EndDate { get; set;}
        }
        public class SalesPersonRequest
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
        }


    }
}
