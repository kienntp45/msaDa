using MicroServices.Samples.Services.Ordering.API.Application.Models;
using MicroServices.Samples.Services.Ordering.API.Application.Repository;
using MicroServices.Samples.Services.Ordering.API.Database;
using MicroServices.Samples.Services.Ordering.API.Database.InMemory;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.Samples.Services.Ordering.API.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _db;
    private readonly ILogger<OrderRepository> _logger;
    private readonly OrderInMemoryContext _inMem;

    public OrderRepository(OrderDbContext db, ILogger<OrderRepository> logger, OrderInMemoryContext inMem)
    {
        _db = db;
        _logger = logger;
        _inMem = inMem;
    }
    public async Task<Order> AddAsync(Order order)
    {
        try
        {
            order.Id = Guid.NewGuid().ToString();
            foreach(var item in order.Items)
            {
                item.Id = Guid.NewGuid().ToString();
            }
            _inMem.Orders.Add(order.Id, order);
            return await Task.FromResult(order);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task<Order> DeleteAsync(string orderId)
    {
        Order order = await GetByIdAsync(orderId);
        try
        {
            if (order != null)
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
            }
            return order;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task<List<Order>> GetByCustomerIdAsync(string customerId)
    {
        List<Order> orders = new List<Order>();
        try
        {
            orders = await _db.Orders.Where(o => o.CustomerId.Equals(customerId)).ToListAsync();
            if (orders != null)
            {
                foreach (var order in orders)
                {
                    await _db.Entry(order).Collection(o => o.Items).LoadAsync();
                }
            }
            return orders;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return orders;
        }
    }

    public async Task<Order> GetByIdAsync(string id)
    {
        Order order = new Order();
        try
        {
            order = _db.Orders.FirstOrDefault(o => o.Id.Equals(id));
            if (order != null)
            {
                await _db.Entry(order).Collection(o => o.Items).LoadAsync();
            }
            return order;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return order;
        }
    }
}