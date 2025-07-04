using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessLayer.Entities;

public class OrderItem
{
    // The unique identifier for the document in MongoDB
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid _id { get; set; }

    // Unique identifier for the product
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid ProductID { get; set; }

    // The price of one unit of the product
    [BsonRepresentation(MongoDB.Bson.BsonType.Double)]
    public decimal UnitPrice { get; set; }

    // The quantity of the product ordered
    [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
    public int Quantity { get; set; }

    // The total price for the ordered quantity of the product
    [BsonRepresentation(MongoDB.Bson.BsonType.Double)]
    public decimal TotalPrice { get; set; }
}
