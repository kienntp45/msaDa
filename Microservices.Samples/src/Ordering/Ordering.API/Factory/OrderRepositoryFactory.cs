using MicroServices.Samples.Services.Ordering.API.Application.Repository;
using MicroServices.Samples.Services.Ordering.API.Database;
using MicroServices.Samples.Services.Ordering.API.Database.InMemory;
using MicroServices.Samples.Services.Ordering.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.Samples.Services.Ordering.API.Factory;


public class OrderRepositoryFactory:IOrderRepositoryFactory
{
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;

    public OrderRepositoryFactory(IServiceProvider services, IConfiguration configuration)
    {
        _services = services;
        _configuration = configuration;
    }
    public IOrderRepository CreateOrderRepository()
    {
        var logger = _services.GetRequiredService<ILogger<OrderRepository>>();
        var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>()
            .UseOracle(_configuration.GetConnectionString("OracleConnectionString"));
            var orderDbContext = new OrderDbContext(optionsBuilder.Options);
        var InMemory = _services.GetService<OrderInMemoryContext>();
        return new OrderRepository(orderDbContext,logger,InMemory);
    }
}