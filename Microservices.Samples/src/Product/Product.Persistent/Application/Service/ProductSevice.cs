using MicroServices.Samples.Services.Product.ProductPersistent.Application.Model;
using MicroServices.Samples.Services.Product.ProductPersistent.Application.Repository;
using Microsoft.Extensions.Logging;
#nullable enable

namespace MicroServices.Samples.Services.Product.ProductPersistent.Application.Service;


public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository repository, ILogger<ProductService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<List<ProductItem>> AddAsync(List<ProductItem> productItems)
    {
        return await _repository.AddAsync(productItems);
    }

    public async Task<List<ProductItem>> DeleteAsync(List<ProductItem> productItems)
    {
        return await _repository.DeleteAsync(productItems);
    }

    public async Task<ProductItem> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<ProductItem>> UpdateAsync(List<ProductItem> productItems)
    {
        return await _repository.UpdateAsync(productItems);
    }

    public async Task<ProductItem> UpdateAvailableQuantityAsync(int id, int quantity)
    {
        var productItem = await _repository.UpdateAvailableQuantityAsync(id, quantity);
        return productItem;
    }

    public async Task<ProductItem> UpdateNameAsync(int id, string name)
    {
        return await _repository.UpdateNameAsync(id, name);
    }

    public async Task<ProductItem> UpdatePriceAsync(int id, decimal price)
    {
        return await _repository.UpdatePriceAsync(id, price);
    }
}