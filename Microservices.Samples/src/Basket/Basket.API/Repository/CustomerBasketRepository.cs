using MicroServices.Samples.Services.Basket.API.Application.Models;
using MicroServices.Samples.Services.Basket.API.Application.Repository;
using MicroServices.Samples.Services.Basket.API.Database;
using MicroServices.Samples.Services.Basket.API.Database.InMemory;

namespace MicroServices.Samples.Services.Basket.API.Repository;


public class CustomerBasketRepository : ICustomerBasketRepository
{
    private readonly BasketDbConText _db;
    private readonly ILogger<CustomerBasketRepository> _logger;
    private readonly BasketInMemoryContext _inMem;

    public CustomerBasketRepository(BasketDbConText db, ILogger<CustomerBasketRepository> logger, BasketInMemoryContext inMem)
    {
        _db = db;
        _logger = logger;
        _inMem = inMem;
    }
    public async Task<CustomerBasket> AddAsync(CustomerBasket customerBasket)
    {
        foreach (var item in customerBasket.Items)
        {
            item.Id = Guid.NewGuid().ToString();

        }
        _inMem.customerBaskets.Add(customerBasket.CusTomerId, customerBasket);
        return await Task.FromResult(customerBasket);
    }
    public async Task<CustomerBasket> UpdateQuantityAsync(string customerId, int quantity, int productId)
    {
        CustomerBasket customerBasket = await GetByIdAsync(customerId);
        var item = customerBasket.Items.Where(i => i.ProductId == productId).FirstOrDefault();
        item.Quantity = quantity;
        return customerBasket;
    }

    public async Task<CustomerBasket> AddBasketItemAsync(CustomerBasket customerBasket)
    {
        CustomerBasket customerBasketDb = await GetByIdAsync(customerBasket.CusTomerId);
        if (customerBasketDb != null)
        {
            foreach (var item in customerBasket.Items)
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    item.Id = Guid.NewGuid().ToString();
                }
            }
            customerBasketDb.Items.AddRange(customerBasket.Items);
        }
        return customerBasket;
    }
    public async Task<CustomerBasket> GetByIdAsync(string customerId)
    {
        CustomerBasket customerBasket = new CustomerBasket();
        var rs = _inMem.customerBaskets.TryGetValue(customerId, out customerBasket);
        if (rs)
        {
            return await Task.FromResult(customerBasket);
        }
        return customerBasket;
    }

    public async Task<CustomerBasket> DeleteBasketItemAsync(string customerId, int productId)
    {
        try
        {
            CustomerBasket customerBasket = await GetByIdAsync(customerId);
            if (customerBasket != null)
            {
                var item = customerBasket.Items.Where(i => i.ProductId == productId).FirstOrDefault();
                if (item != null)
                {
                    customerBasket.Items.Remove(item);
                }
            }
            return customerBasket;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }

    }

    public async Task<CustomerBasket> DeleteAllBasketItemAsync(string customerId)
    {
        try
        {
            CustomerBasket customerBasket = await GetByIdAsync(customerId);
            if (customerBasket != null)
            {
                foreach (var item in customerBasket.Items)
                {
                    customerBasket.Items.Remove(item);
                }
            }
            return customerBasket;
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

    public async Task<List<CustomerBasket>> ListAsync()
    {
        List<CustomerBasket> customerBaskets = new List<CustomerBasket>();
        try
        {
            customerBaskets = _inMem.customerBaskets.Values.ToList();

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
        return await Task.FromResult(customerBaskets);
    }
    public async Task<CustomerBasket> UpdateStatusAsync(string customerId, int status)
    {
        var customerBasket = await GetByIdAsync(customerId);
        try
        {
            if (customerBasket != null)
            {
                foreach (var item in customerBasket.Items)
                {
                    item.Status = status;
                }
            }
            return customerBasket;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }
}
