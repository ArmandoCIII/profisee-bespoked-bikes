using Amazon.DynamoDBv2.DataModel;
namespace SalesPersonManager.Models
{
    [DynamoDBTable("SalesPerson")]
    public class SalesPerson
    {
        [DynamoDBHashKey("FirstName")]
        public string? FirstName { get; set; }
        [DynamoDBRangeKey("LastName")]
        public string? LastName { get; set; }
        [DynamoDBProperty("Address")]
        public string? Address { get; set; }
        [DynamoDBProperty("Manager")]
        public string? Manager { get; set; }
        [DynamoDBProperty("Phone")]
        public long? Phone { get; set; }
        [DynamoDBProperty("StartDate")]
        public DateMap? StartDate { get; set; }
        [DynamoDBProperty("TerminationDate")]
        public DateMap? TerminationDate { get; set; }
    }

    public class DateMap
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
