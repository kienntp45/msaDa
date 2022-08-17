using MicroServices.Samples.Services.Ordering.API.Application.Models;
using MicroServices.Samples.Services.Ordering.API.DTOs;

namespace MicroServices.Samples.Services.Ordering.API.Application.Service;


public interface IOrderService 
{
    Task<UpsertOrderResponse> AddAsync(UpsertOrder upsertOrder);
    Task<UpsertOrderResponse> DeleteAsync(string orderId);
    Task<List<Order>> GetByCustomerIdAsync(string customerId);
    Task<Order> GetByIdAsync(string orderId);
}