using MicroServices.Samples.Services.Product.API.Application.Models;
using MicroServices.Samples.Services.Product.API.DTOs;
#nullable enable
namespace MicroServices.Samples.Services.Product.API.Application.Repository;


public interface IProductRepository
{
    Task<List<ProductItem>> ListAsync(string? name,decimal? minPrice,decimal? maxPrice,bool? sortPrice);
    Task<ProductItem> AddAsync(ProductItem productItem);
    Task<ProductItem> GetByIdAsync(int id);
    Task<ProductItem> UpdateNameAsync(int id, string name);
    Task<ProductItem> UpdateAvailableQuantityAsync(int id, int quantity);
    Task<ProductItem> UpdatePriceAsync(int id, decimal price);
    Task<ProductItem> UpdateAsync(ProductItem productItem);
    Task<ProductItem> DeleteAsync(int id);
}