using MicroServices.Samples.Services.Product.API.Application.Models;
using MicroServices.Samples.Services.Product.API.Application.Repository;
using MicroServices.Samples.Services.Product.API.Database;

namespace MicroServices.Samples.Services.Product.API.Repository;


public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _db;
    private readonly ILogger<ProductRepository> _logger;
    private readonly ProductInMemoryContext _inMem;

    public ProductRepository(ProductDbContext db, ILogger<ProductRepository> logger, ProductInMemoryContext inMem)
    {
        _db = db;
        _logger = logger;
        _inMem = inMem;
    }
    public async Task<ProductItem> AddAsync(ProductItem productItem)
    {
        try
        {
            if (productItem != null)
            {
                _db.Products.Add(productItem);
                await _db.SaveChangesAsync();
            }
            return productItem;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task<ProductItem> DeleteAsync(int id)
    {
        ProductItem productItem = await GetByIdAsync(id);
        try
        {
            if (productItem != null)
            {
                _inMem.Products.Remove(id);
            }
            return productItem;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task<ProductItem> GetByIdAsync(int id)
    {
        var rs = _inMem.Products.TryGetValue(id, out ProductItem productItem);
        if (rs)
            return await Task.FromResult(productItem);
        var data = await _db.Products.FindAsync(id);
        if (data != null)
            _inMem.Products.TryAdd(id, data);
        return data;
    }

    public async Task<List<ProductItem>> ListAsync(string name, decimal? minPrice, decimal? maxPrice, bool? sortPrice)
    {
        List<ProductItem> listProductItem = new List<ProductItem>();
        try
        {
            var query = _inMem.Products.Values.Where(p => string.IsNullOrEmpty(name) || p.Name.Contains(name))
            .Where(p => minPrice == null || p.Price >= minPrice)
            .Where(p => maxPrice == null || p.Price <= maxPrice);
            if (sortPrice != null)
            {
                if (!sortPrice.Value)
                    listProductItem = query.OrderByDescending(p => p.Price).ToList();
                if (sortPrice.Value)
                    listProductItem = query.OrderBy(p => p.Price).ToList();
            }
            else
                listProductItem = query.ToList();

            return await Task.FromResult(listProductItem);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return await Task.FromResult(listProductItem);
        }
    }

    public async Task<ProductItem> UpdateAsync(ProductItem productItem)
    {
        try
        {
            if (productItem != null)
            {
                _db.Products.Update(productItem);
                await _db.SaveChangesAsync();
            }
            return productItem;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task<ProductItem> UpdateAvailableQuantityAsync(int id, int quantity)
    {
        ProductItem productItem = await GetByIdAsync(id);
        try
        {
            if (productItem != null)
            {
                _inMem.Products[id].AvailableQuantity = quantity;
                return productItem;
            }
            return productItem;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task<ProductItem> UpdateNameAsync(int id, string name)
    {
        ProductItem productItem = await GetByIdAsync(id);
        try
        {
            if (productItem != null)
            {
                _inMem.Products[id].Name = name;
                return productItem;
            }
            return productItem;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task<ProductItem> UpdatePriceAsync(int id, decimal price)
    {
        ProductItem productItem = await GetByIdAsync(id);
        try
        {
            if (productItem != null)
            {
                _inMem.Products[id].Price = price;
                return productItem;
            }
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
}