using MicroServices.Samples.Services.Basket.API.Database;
using MicroServices.Samples.Services.Basket.API.Database.InMemory;
using MicroServices.Samples.Services.Basket.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.Samples.Services.Basket.API.Factory;


public class CustomerBasketRepositoryFactory : ICustomerBasketRepositoryFactory
{
    private readonly IServiceProvider _services;
    private readonly IConfiguration _config;

    public CustomerBasketRepositoryFactory(IServiceProvider services, IConfiguration config)
    {
        _services = services;
        _config = config;
    }
    public CustomerBasketRepository CreateCustomerBasketRepository()
    {
        var logger = _services.GetRequiredService<ILogger<CustomerBasketRepository>>();
        var optionsBuilder = new DbContextOptionsBuilder<BasketDbConText>()
            .UseOracle(_config.GetConnectionString("OracleConnectionString"));
        var basketDbContext = new BasketDbConText(optionsBuilder.Options);
        var inMem = _services.GetService<BasketInMemoryContext>();
        return new CustomerBasketRepository(basketDbContext, logger,inMem);
    }
}