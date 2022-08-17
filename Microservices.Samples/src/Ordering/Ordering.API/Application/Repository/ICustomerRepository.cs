using MicroServices.Samples.Services.Ordering.API.Application.Models;

namespace MicroServices.Samples.Services.Ordering.API.Application.Repository;


public interface ICustomerRepository
{
    public Task<List<Customer>> GetByIdentityAsync(string identity);
    public Task<Customer> AddAsync(Customer customer);
}