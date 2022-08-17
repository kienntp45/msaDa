using MicroServices.Samples.Services.Ordering.API.Application.Models;

namespace MicroServices.Samples.Services.Ordering.API.Database.InMemory;


public class CustomerInMemoryContext
{
    public  Dictionary<string, Customer> Customers {get;set;}

    public CustomerInMemoryContext ()
    {
        Customers = new Dictionary<string, Customer>();
    }
}