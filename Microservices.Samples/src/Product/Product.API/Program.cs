using Microsoft.EntityFrameworkCore;
using MicroServices.Samples.Services.Product.API.Application.Repository;
using MicroServices.Samples.Services.Product.API.Application.Service;
using MicroServices.Samples.Services.Product.API.Database;
using MicroServices.Samples.Services.Product.API.Extensions;
using MicroServices.Samples.Services.Product.API.Repository;
using Confluent.Kafka;
using MicroServices.Samples.Services.Product.API.Config;
using MicroServices.Samples.Shared.Common.Utils;
using MicroServices.Samples.Services.Product.API.BackgroundTasks;
using MicroServices.Samples.Services.Product.API.Factory;
using NetMQ.Sockets;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var kafkaConfig = configuration.GetSection("KafkaConfig").Get<KafkaConfig>();
// Add services to the container.
var producerConfig = new ProducerConfig
{
    BootstrapServers = kafkaConfig.BootstrapServers,
    QueueBufferingMaxMessages = kafkaConfig.QueueBufferingMaxMessages,
    MessageSendMaxRetries = kafkaConfig.MessageSendMaxRetries,
    RetryBackoffMs = kafkaConfig.RetryBackOffMs,
    LingerMs = kafkaConfig.LingerMs,
    DeliveryReportFields = kafkaConfig.DeliveryReportFields
};
var consumerConfig = new ConsumerConfig
{
    GroupId = "ProductService",
    BootstrapServers = kafkaConfig.BootstrapServers,
    SessionTimeoutMs = 6000,
    QueuedMinMessages = 1000000
};

builder.Services.AddControllers();
builder.Services.AddSingleton<ProductInMemoryContext>();
builder.Services.AddOracle<ProductDbContext>(configuration.GetConnectionString("OracleConnectionString"));
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<ConsumerBackgroundTask>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddSingleton(sp =>
{
    var producer = new ProducerBuilder<string, string>(producerConfig).Build();
    return new KafkaProducer<string, string>(producer);
}
);
builder.Services.AddSingleton(sp =>
{
    var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    return new KafkaProducer<Null, string>(producer);
}
);
builder.Services.AddSingleton(sp =>
{
    var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
    return new KafkaConsumer<Ignore, string>(consumer);
});
builder.Services.AddSingleton(sp =>
{
    var pullSocket = new PullSocket();
    pullSocket.Bind(configuration["NetMQConfig1"]);
    return new NetMQPull(pullSocket);
});
builder.Services.AddSingleton(sp =>
{
    // configuration.GetSection("NetMQConfig").ToString()
    var pushSocket = new PushSocket();
    pushSocket.Connect(configuration["NetMQConfig"]);
    return new NetMQPush(pushSocket);
});
builder.Services.AddSingleton(sp =>
{
    return new ProductServiceFactory(sp, builder.Configuration);
});
builder.Services.AddSingleton<InMemoryRequestManagement>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MigrateDbContext<ProductDbContext>((context, services) =>
{
    var logger = services.GetRequiredService<ILogger<ProductDbContextSeed>>();
    new ProductDbContextSeed()
        .SeedAsync(context, logger)
        .Wait();
});

app.LoadDataToMemory<ProductInMemoryContext, ProductDbContext>((inMemoryContext, dbContext) =>
{
    new ProductInMemoryContextSeed().SeedAsync(inMemoryContext, dbContext).Wait();
});

app.Run();
