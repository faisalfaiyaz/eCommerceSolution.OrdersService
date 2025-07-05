using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using MongoDB.Driver;

namespace DataAccessLayer.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly IMongoCollection<Order> _order;
    public OrdersRepository(IMongoDatabase mongoDatabase)
    {
        _order = mongoDatabase.GetCollection<Order>("orders");
        
    }
    public async Task<Order?> AddOrder(Order order)
    {
        order.OrderID = Guid.NewGuid();
        await _order.InsertOneAsync(order);
        return order;
    }

    public async Task<bool> DeleteOrder(Guid orderID)
    {
        FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(o => o.OrderID, orderID);

        Order? existingOrder = (await _order.FindAsync(filter)).FirstOrDefault();

        if (existingOrder is null)
        {
            return false;
        }

        DeleteResult deleteResult = await _order.DeleteOneAsync(filter);

        return deleteResult.DeletedCount > 0;
    }

    public async Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter)
    {
        return (await _order.FindAsync(filter)).FirstOrDefault();
    }

    public async Task<IEnumerable<Order>> GetOrders()
    {
        return (await _order.FindAsync(Builders<Order>.Filter.Empty)).ToList();
    }

    public async Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter)
    {
        return (await _order.FindAsync(filter)).ToList();
    }

    public async Task<Order?> UpdateOrder(Order order)
    {
        FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(o => o.OrderID, order.OrderID);

        Order? existingOrder = (await _order.FindAsync(filter)).FirstOrDefault();

        if (existingOrder is null)
        {
            return null;
        }

        ReplaceOneResult replaceOneResult = await _order.ReplaceOneAsync(filter, order);

        // return replaceOneResult.ModifiedCount == 1 ? order : null;
        return order;
    }
}
