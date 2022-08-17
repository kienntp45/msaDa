using MicroServices.Samples.Services.Basket.API.Application.Models;
using MicroServices.Samples.Services.Basket.API.DTOs;

namespace MicroServices.Samples.Services.Basket.API.Application.Service;


public interface ICustomerBasketService
{
    Task<CustomerBasket> GetByIdAsync(string customerId);
    Task<UpsertCustomerBasketResponseDTO> AddAsync(UpsertCustomerBasketDTO upsertCustomerBasketDTO);
    Task<UpsertCustomerBasketResponseDTO> UpdateStatusAsync(UpsertStatusDTO UpsertStatusDTO);
    Task<List<CustomerBasket>> ListAsync();
    
}