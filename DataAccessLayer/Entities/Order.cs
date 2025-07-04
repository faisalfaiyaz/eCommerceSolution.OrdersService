using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessLayer.Entities;

public class Order
{
    // The unique identifier for the document in MongoDB
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid _id { get; set; }

    // Unique identifier for the order
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid OrderID { get; set; }


    // Unique identifier for the user who placed the order
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid UserID { get; set; }


    // The date when the order was placed
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public DateTime OrderDate { get; set; }


    // The total amount billed for the order
    [BsonRepresentation(MongoDB.Bson.BsonType.Double)]
    public decimal TotalBill { get; set; }


    // List of items included in the order
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
