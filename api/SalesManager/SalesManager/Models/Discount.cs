using Amazon.DynamoDBv2.DataModel;

namespace SalesManager.Models
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
}
