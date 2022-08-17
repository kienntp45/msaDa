using MicroServices.Samples.Services.Ordering.API.Application.Models;
using MicroServices.Samples.Services.Ordering.API.Application.Repository;
using MicroServices.Samples.Services.Ordering.API.Database;
using MicroServices.Samples.Services.Ordering.API.Database.InMemory;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.Samples.Services.Ordering.API.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly OrderDbContext _db;
    private readonly ILogger<CustomerRepository> _logger;
    private readonly CustomerInMemoryContext _inMem;

    public CustomerRepository(OrderDbContext db, ILogger<CustomerRepository> logger, CustomerInMemoryContext inMem)
    {
        _db = db;
        _logger = logger;
        _inMem = inMem;
    }
    public async Task<Customer> AddAsync(Customer customer)
    {
        try
        {
            customer.Id = Guid.NewGuid().ToString();
            _inMem.Customers.Add(customer.Id, customer);
            return await Task.FromResult(customer);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task<List<Customer>> GetByIdentityAsync(string identity)
    {
        List<Customer> customers = new List<Customer>();
        try
        {
            if (!string.IsNullOrEmpty(identity))
            {
                customers = await _db.Customers.Where(c => c.IdentityId.Equals(identity)).ToListAsync();
            }
            return customers;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return customers;
        }
    }
}
