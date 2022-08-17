using MicroServices.Samples.Services.Ordering.API.Application.Models;
using MicroServices.Samples.Services.Ordering.API.Application.Repository;

namespace MicroServices.Samples.Services.Ordering.API.Application.Service;


public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(ICustomerRepository repository, ILogger<CustomerService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<Customer> AddAsync(Customer customer)
    {
        Customer customer1 = await _repository.AddAsync(customer);
        return customer1;
    }

    public async Task<List<Customer>> GetByIdentityAsync(string identity)
    {
        List<Customer> customer = new List<Customer>();
        if (!string.IsNullOrEmpty(identity))
        {
            customer = await _repository.GetByIdentityAsync(identity);
        }
        return customer;
    }
}
