using MicroServices.Samples.Services.Ordering.API.Application.Models;

namespace MicroServices.Samples.Services.Ordering.API.Application.Repository;

public interface IOrderRepository
{
    Task<Order> AddAsync(Order order);
    Task<Order> DeleteAsync( string orderId);
    Task<List<Order>> GetByCustomerIdAsync(string customerId);
    Task<Order> GetByIdAsync(string id);
}