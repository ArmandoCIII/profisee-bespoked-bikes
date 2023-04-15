using Amazon.DynamoDBv2.DataModel;
namespace ProductManager.Models
{
    [DynamoDBTable("Products")]
    public class Products
    {
        [DynamoDBHashKey("Name")]
        public string? Name { get; set; }
        [DynamoDBProperty("Manufacturer")]
        public string? Manufacturer { get; set; }
        [DynamoDBProperty("Style")]
        public string? Style { get; set; }
        [DynamoDBProperty("PurchasePrice")]
        public double? PurchasePrice { get; set; }
        [DynamoDBProperty("SalePrice")]
        public double? SalePrice { get; set; }
        [DynamoDBProperty("QtyOnHand")]
        public int? QtyOnHand { get; set; }
    }
}
