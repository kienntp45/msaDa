using Confluent.Kafka;
using MicroServices.Samples.Services.Product.ProductPersistent.Application.Repository;
using MicroServices.Samples.Services.Product.ProductPersistent.Application.Service;
using MicroServices.Samples.Services.Product.ProductPersistent.BackgroundTasks;
using MicroServices.Samples.Services.Product.ProductPersistent.Config;
using MicroServices.Samples.Services.Product.ProductPersistent.Database;
using MicroServices.Samples.Services.Product.ProductPersistent.Disruptor;
using MicroServices.Samples.Services.Product.ProductPersistent.Factory;
using MicroServices.Samples.Services.Product.ProductPersistent.Repository;
using MicroServices.Samples.Shared.Common.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
.AddJsonFile("appsettings.json", true, true)
.AddEnvironmentVariables();
var configuration = configurationBuilder.Build();
var kafkaConfig = configuration.GetSection("KafkaConfig").Get<KafkaConfig>();
var consumerConfig = new ConsumerConfig
{
    GroupId = "ProductPersistent",
    BootstrapServers = kafkaConfig.BootstrapServers,
    SessionTimeoutMs = 6000,
    QueuedMinMessages = 1000000
};
var oracle = configuration["OracleConnectionString"];
var host = Host.CreateDefaultBuilder();
host.ConfigureServices((_, services)
   =>
{
    services.AddScoped<IProductService, ProductService>();
    services.AddOracle<ProductDbContext>(configuration.GetConnectionString("OracleConnectionString"), o => o
            .MinBatchSize(1)
            .MaxBatchSize(100));
    services.AddScoped<IProductRepository, ProductRepository>();
    services.AddSingleton<ProductServiceFactory>();
    services.AddHostedService<ProductBackgroundTask>();
    services.AddSingleton<ProductKafkaMessageRingBuffer>();
    services.AddSingleton(sp =>
    {
        var consume = new ConsumerBuilder<string, string>(consumerConfig).Build();
        return new KafkaConsumer<string, string>(consume);
    });
    services.AddSingleton<IDbContextFactory<ProductDbContext>>(sp => {
        return new ProductDbContextFactory(configuration.GetConnectionString("OracleConnectionString"));
    });
});
var app = host.Build();
app!.Run();