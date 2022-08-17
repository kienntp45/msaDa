using Confluent.Kafka;
using MicroServices.Samples.Services.Ordering.API.Application.Repository;
using MicroServices.Samples.Services.Ordering.API.Application.Service;
using MicroServices.Samples.Services.Ordering.API.BackgroundTasks;
using MicroServices.Samples.Services.Ordering.API.Config;
using MicroServices.Samples.Services.Ordering.API.Database;
using MicroServices.Samples.Services.Ordering.API.Database.InMemory;
using MicroServices.Samples.Services.Ordering.API.Factory;
using MicroServices.Samples.Services.Ordering.API.Repository;
using MicroServices.Samples.Shared.Common.Utils;
using Microsoft.EntityFrameworkCore;
using NetMQ.Sockets;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var kafkaConfig = configuration.GetSection("KafkaConfig").Get<KafkaConfig>();
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
    GroupId = "OrderingService",
    BootstrapServers = kafkaConfig.BootstrapServers,
    SessionTimeoutMs = 6000,
    QueuedMinMessages = 1000000
};
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<OrderInMemoryContext>();
builder.Services.AddSingleton<CustomerInMemoryContext>();
builder.Services.AddOracle<OrderDbContext>(configuration.GetConnectionString("OracleConnectionString"));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICustomerService,CustomerService> ();
builder.Services.AddScoped<ICustomerRepository,CustomerRepository>();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<ConsumeBackgroundTasks>();
builder.Services.AddSingleton(sp =>
{
    var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    return new KafkaProducer<Null, string>(producer);
});
builder.Services.AddSingleton(sp =>
{
    var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
    return new KafkaConsumer<Ignore, string>(consumer);
});
builder.Services.AddSingleton<IOrderRepositoryFactory,OrderRepositoryFactory>(sp => {
    return new OrderRepositoryFactory(sp, builder.Configuration);
});
builder.Services.AddSingleton<InMemoryRequestManagement>();
builder.Services.AddSingleton(sp=>
{
    var pushSocket = new PushSocket();
    pushSocket.Connect(configuration["NetMQConfig1"]);
    return new NetMQPush(pushSocket);
});
builder.Services.AddSingleton(sp =>
{
    // configuration.GetSection("NetMQConfig").ToString()
    var pullSocket = new PullSocket();
    pullSocket.Bind(configuration["NetMQConfig"]);
    return new NetMQPull(pullSocket);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// app.LoadDataToMemory<CustomerInMemoryContext,OrderDbContext>((cusInMem,dbContext)=>
// {
//     new CustomerInMemoryContextSeed().SeedAsync(cusInMem,dbContext).Wait();
// });
app.Run();
