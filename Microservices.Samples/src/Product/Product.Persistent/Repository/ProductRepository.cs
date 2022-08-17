using MicroServices.Samples.Services.Product.ProductPersistent.Application.Model;
using MicroServices.Samples.Services.Product.ProductPersistent.Application.Repository;
using MicroServices.Samples.Services.Product.ProductPersistent.Database;
using Microsoft.Extensions.Logging;

namespace MicroServices.Samples.Services.Product.ProductPersistent.Repository;


public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _db;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(ProductDbContext db, ILogger<ProductRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<List<ProductItem>> AddAsync(List<ProductItem> productItems)
    {
        try
        {
            await _db.Products.AddRangeAsync(productItems);
            return productItems;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task<List<ProductItem>> DeleteAsync(List<ProductItem> productItems)
    {
        try
        {
            _db.Products.RemoveRange(productItems);
            return await Task.FromResult(productItems);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task<ProductItem> GetByIdAsync(int id)
    {
        try
        {
            var productItem = await _db.Products.FindAsync(id);
            return productItem;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync();
    }

    public async Task<List<ProductItem>> UpdateAsync(List<ProductItem> productItems)
    {
        try
        {
            _db.Products.UpdateRange(productItems);
            return await Task.FromResult(productItems);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task<ProductItem> UpdateAvailableQuantityAsync(int id, int quantity)
    {
        ProductItem productItem = new ProductItem();
        try
        {
            productItem = await GetByIdAsync(id);
            productItem.AvailableQuantity = quantity;
            return productItem;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return productItem;
        }
    }

    public async Task<ProductItem> UpdateNameAsync(int id, string name)
    {
        ProductItem productItem = new ProductItem();
        try
        {
            productItem = await GetByIdAsync(id);
            productItem.Name = name;
            return productItem;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return productItem;
        }
    }

    public async Task<ProductItem> UpdatePriceAsync(int id, decimal price)
    {
        ProductItem productItem = new ProductItem();
        try
        {
            productItem = await GetByIdAsync(id);
            productItem.Price = price;
            return productItem;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return productItem;
        }
    }
}