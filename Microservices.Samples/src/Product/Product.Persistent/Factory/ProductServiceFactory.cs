using MicroServices.Samples.Services.Product.ProductPersistent.Application.Repository;
using MicroServices.Samples.Services.Product.ProductPersistent.Application.Service;
using MicroServices.Samples.Services.Product.ProductPersistent.Database;
using MicroServices.Samples.Services.Product.ProductPersistent.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MicroServices.Samples.Services.Product.ProductPersistent.Factory;

public class ProductServiceFactory
{
    private readonly IServiceProvider _service;
    private readonly IConfiguration _config;

    public ProductServiceFactory(IServiceProvider service, IConfiguration config)
    {
        _service = service;
        _config = config;
    }
    public IProductService CreateService()
    {
        var serviceLogger = _service.GetRequiredService<ILogger<ProductService>>();
        var repoLogger = _service.GetRequiredService<ILogger<ProductRepository>>();
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>()
            .UseOracle(_config.GetConnectionString("OracleConnectionString"));
        var dbContext = new ProductDbContext(optionsBuilder.Options);
        var repository = new ProductRepository(dbContext, repoLogger);
        return new ProductService(repository, serviceLogger);
    }
    public IProductRepository CreateRepository()
    {
        var repoLogger = _service.GetRequiredService<ILogger<ProductRepository>>();
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>()
            .UseOracle(_config.GetConnectionString("OracleConnectionString"));
        var dbContext = new ProductDbContext(optionsBuilder.Options);
        return new ProductRepository(dbContext, repoLogger);
    }
}