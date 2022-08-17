using Confluent.Kafka;
using MicroServices.Samples.Services.Basket.API.Application.Repository;
using MicroServices.Samples.Services.Basket.API.Application.Service;
using MicroServices.Samples.Services.Basket.API.BackgroundTasks;
using MicroServices.Samples.Services.Basket.API.Config;
using MicroServices.Samples.Services.Basket.API.Database;
using MicroServices.Samples.Services.Basket.API.Database.InMemory;
using MicroServices.Samples.Services.Basket.API.Extensions;
using MicroServices.Samples.Services.Basket.API.Factory;
using MicroServices.Samples.Services.Basket.API.Repository;
using MicroServices.Samples.Shared.Common.Utils;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var kafkaConfig = configuration.GetSection("KafkaConfig").Get<KafkaConfig>();
var consumerConfig = new ConsumerConfig
{
    GroupId = "BasketService",
    BootstrapServers = kafkaConfig.BootstrapServers,
    SessionTimeoutMs = 6000,
    QueuedMinMessages = 1000000
};
var producerConfig = new ProducerConfig
{
    BootstrapServers = kafkaConfig.BootstrapServers,
    QueueBufferingMaxMessages = kafkaConfig.QueueBufferingMaxMessages,
    MessageSendMaxRetries = kafkaConfig.MessageSendMaxRetries,
    RetryBackoffMs = kafkaConfig.RetryBackOffMs,
    LingerMs = kafkaConfig.LingerMs,
    DeliveryReportFields = kafkaConfig.DeliveryReportFields
};
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<BasketInMemoryContext>();
builder.Services.AddOracle<BasketDbConText>(builder.Configuration.GetConnectionString("OracleConnectionString"));
builder.Services.AddScoped<ICustomerBasketService, CustomerBasketService>();
builder.Services.AddScoped<ICustomerBasketRepository, CustomerBasketRepository>();
builder.Services.AddHostedService<ConsumerBackgroundTasks>();
builder.Services.AddSingleton<ICustomerBasketRepositoryFactory, CustomerBasketRepositoryFactory>(sp =>
{
    return new CustomerBasketRepositoryFactory(sp, builder.Configuration);
});
builder.Services.AddSingleton(sp =>
{
    var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
    return new KafkaConsumer<Ignore, string>(consumer);
});
builder.Services.AddSingleton(sp =>
{
    var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    return new KafkaProducer<Null, string>(producer);
});
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.LoadDataInMemory<BasketInMemoryContext, BasketDbConText>((basketInMe, dbContext) =>
{
    new BasketInMemoryContextSeed().SeedAsync(basketInMe, dbContext).Wait();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
