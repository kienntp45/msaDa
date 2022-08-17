using MicroServices.Samples.Services.Ordering.API.Application.Models;

namespace MicroServices.Samples.Services.Ordering.API.Application.Service;

public interface ICustomerService
{
    public Task<List<Customer>> GetByIdentityAsync(string identity);
    public Task<Customer> AddAsync(Customer customer);
}