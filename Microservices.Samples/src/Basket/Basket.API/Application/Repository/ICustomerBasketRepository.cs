using MicroServices.Samples.Services.Basket.API.Application.Models;

namespace MicroServices.Samples.Services.Basket.API.Application.Repository;

public interface ICustomerBasketRepository
{
    Task<CustomerBasket> GetByIdAsync(string customerId);
    Task<CustomerBasket> AddAsync(CustomerBasket customerBasket);
    Task<CustomerBasket> AddBasketItemAsync(CustomerBasket customerBasket);
    Task<CustomerBasket> UpdateQuantityAsync(string customerId, int quantity, int productId);
    Task<CustomerBasket> DeleteBasketItemAsync(string customerId, int productId);
    Task<CustomerBasket> UpdateStatusAsync(string customerId,int status);
    Task<List<CustomerBasket>> ListAsync();
}