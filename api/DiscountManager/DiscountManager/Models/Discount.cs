using Amazon.DynamoDBv2.DataModel;

namespace DiscountManager.Models
{
    [DynamoDBTable("Discount")]
    public class Discount
    {
        [DynamoDBHashKey("Product")]
        public string? Product { get; set; }
        [DynamoDBProperty("BeginDate")]
        public DateMap? BeginDate { get; set; }
        [DynamoDBProperty("EndDate")]
        public DateMap? EndDate { get; set; }
        [DynamoDBProperty("DiscountPercentage")]
        public int? DiscountPercentage { get; set; }
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
