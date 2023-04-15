using Amazon.DynamoDBv2.DataModel;

namespace SalesManager.Models
{
    [DynamoDBTable("Sales")]
    public class Sales
    {
        [DynamoDBHashKey("Product")]
        public string? Product { get; set; }
        [DynamoDBProperty("Customer")]
        public Customer? Customer { get; set; }
        [DynamoDBProperty("SalesPerson")]
        public SalesPerson? SalesPerson { get; set; }
        [DynamoDBProperty("SalesDate")]
        public DateMap? SalesDate { get; set; }
    }

    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class SalesPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class DateMap
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public TimeMap? Time { get; set; }
    }

    public class TimeMap
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

    }
}
