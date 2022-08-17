using MicroServices.Samples.Services.Product.API.Application.Service;
using MicroServices.Samples.Services.Product.API.Database;
using MicroServices.Samples.Services.Product.API.Repository;
using MicroServices.Samples.Shared.Common.Utils;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.Samples.Services.Product.API.Factory;

public class ProductServiceFactory
{
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;

    public ProductServiceFactory(IServiceProvider services, IConfiguration configuration)
    {
        _services = services;
        _configuration = configuration;
    }
    public IProductService CreateProductService()
    {
        var serviceLogger = _services.GetRequiredService<ILogger<ProductService>>();
        var repoLogger = _services.GetRequiredService<ILogger<ProductRepository>>();
        var kafkaProducer = _services.GetRequiredService<KafkaProducer<string,string>>();
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>()
            .UseOracle(_configuration.GetConnectionString("OracleConnectionString"));
        var dbContext = new ProductDbContext(optionsBuilder.Options);
        var inMemoryContext = _services.GetService<ProductInMemoryContext>();
        var productRepo = new ProductRepository(dbContext, repoLogger,inMemoryContext);
        return new ProductService(productRepo, serviceLogger,kafkaProducer);
    }
}