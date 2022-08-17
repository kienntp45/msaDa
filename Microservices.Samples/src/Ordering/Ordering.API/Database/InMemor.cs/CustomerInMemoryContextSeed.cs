using Microsoft.EntityFrameworkCore;

namespace MicroServices.Samples.Services.Ordering.API.Database.InMemory;


public class CustomerInMemoryContextSeed
{
    public async Task SeedAsync(CustomerInMemoryContext inMemoryContext, OrderDbContext orderDbContext)
    {
        var customer = await orderDbContext.Customers.ToListAsync();
        foreach (var data in customer)
        {
            inMemoryContext.Customers.Add(data.Id, data);
        }
    }
}