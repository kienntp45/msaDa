using System.Text.Json;
using Confluent.Kafka;
using MicroServices.Samples.Services.Ordering.API.Application.Commands;
using MicroServices.Samples.Services.Ordering.API.Factory;
using MicroServices.Samples.Shared.Common.Utils;

namespace MicroServices.Samples.Services.Ordering.API.BackgroundTasks;


public class ConsumeBackgroundTasks : BackgroundService
{
    private readonly KafkaConsumer<Ignore, string> _kafkaConsumer;
    private readonly IOrderRepositoryFactory _orderRepositoryFactory;
    private readonly InMemoryRequestManagement _requestManagement;
    private readonly NetMQPull _netMQPull;

    public ConsumeBackgroundTasks(KafkaConsumer<Ignore, string> kafkaConsumer, IOrderRepositoryFactory orderRepositoryFactory, InMemoryRequestManagement requestManagement,
    NetMQPull netMQPull)
    {
        _kafkaConsumer = kafkaConsumer;
        _orderRepositoryFactory = orderRepositoryFactory;
        _requestManagement = requestManagement;
        _netMQPull = netMQPull;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var task = new TaskFactory().StartNew(() =>
            _netMQPull.ReceiveMessage(callback, cancellationTokenSource.Token), cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        return task;
    }
    public void callback(string message)
    {
        ProductUpdateQuantityResponseCommand productUpdateQuantityResponseCommand
        = JsonSerializer.Deserialize<ProductUpdateQuantityResponseCommand>(message);
        _requestManagement.SetResponse(productUpdateQuantityResponseCommand.Id, productUpdateQuantityResponseCommand);
    }
}