using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Http;
using SalesManager.Models;
using Microsoft.AspNetCore.Mvc;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using Amazon.Runtime;

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
            var credentials = new StoredProfileAWSCredentials("bespoked-bikes-webapi-dynamodb");
            _dynamoDBContext = dynamoDBContext;
            _dynamoDBClient = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
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
            var sales = await _dynamoDBContext.LoadAsync<Sales>(newSales.Product);
            if (sales != null) return BadRequest($"Sales Already Exists");
            await _dynamoDBContext.SaveAsync(newSales);
            return Ok(newSales);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSales(Sales newSales)
        {
            var sales = await _dynamoDBContext.LoadAsync<Sales>(newSales.Product);
            if (sales == null) return BadRequest($"Sales Does Not Exist");
            await _dynamoDBContext.SaveAsync(newSales);
            return Ok(newSales);
        }
        [HttpPost("dateRange")]
        public async Task<IActionResult> FilterDateRange([FromBody] DateRangeRequest body)
        {
            /*Idealy I would be able to make some sort of comparison here as I tried to do in the FilterExpression.
            Unfortunately I kept running into a scalar error that I would need to figure out. I would then create some form of this method
            and place it in a class to be able to use in both FilterDate Range and SalesPersonCommissionReport.*/
            var startDate = new DateTime(body.StartDate.Year, body.StartDate.Month, body.StartDate.Day, 0, 0, 0, DateTimeKind.Utc);
            var endDate = new DateTime(body.EndDate.Year, body.EndDate.Month, body.EndDate.Day, 23, 59, 59, DateTimeKind.Utc);

            var request = new ScanRequest
            {
                TableName = "Sales",
                FilterExpression = "#salesDate.#year between :startYear and :endYear and #salesDate.#month between :startMonth and :endMonth and #salesDate.#day between :startDay and :endDay",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#salesDate", "SalesDate"},
                    {"#year", "Year"},
                    {"#month", "Month"},
                    {"#day", "Day"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":startYear", new AttributeValue {N = startDate.Year.ToString()}},
                    {":endYear", new AttributeValue {N = endDate.Year.ToString()}},
                    {":startMonth", new AttributeValue {N = startDate.Month.ToString()}},
                    {":endMonth", new AttributeValue {N = endDate.Month.ToString()}},
                    {":startDay", new AttributeValue {N = startDate.Day.ToString()}},
                    {":endDay", new AttributeValue {N = endDate.Day.ToString()}}
                }
            };

            var response = await _dynamoDBClient.ScanAsync(request);
            var sales = response.Items.Select(item => _dynamoDBContext.FromDocument<Sales>(Document.FromAttributeMap(item)));
            return Ok(sales);
        }
        [HttpPost("SalesPerson")]
        public async Task<IActionResult> SalesPersonCommissionReport([FromBody] SalesPersonRequest body)
        {


            var firstName = body.FirstName;
            var lastName = body.LastName;

            var request = new ScanRequest
            {
                TableName = "Sales",
                FilterExpression = "#SalesPerson.#FirstName = :firstName AND #SalesPerson.#LastName = :lastName",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#SalesPerson", "SalesPerson"},
                    {"#FirstName", "FirstName"},
                    {"#LastName", "LastName"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":firstName", new AttributeValue {S = firstName}},
                    {":lastName", new AttributeValue {S = lastName}}
                }

            };
            
            //Would include some sort of quarterly fitler for the Items returned.


            var response = await _dynamoDBClient.ScanAsync(request);
            var sales = response.Items.Select(item => _dynamoDBContext.FromDocument<Sales>(Document.FromAttributeMap(item)));
            var salesPersonCommissionSum = sales.Sum(s => s.SalesPersonCommission);
            return Ok(salesPersonCommissionSum);
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
