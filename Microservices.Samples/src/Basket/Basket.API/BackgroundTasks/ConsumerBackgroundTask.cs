using System.Text.Json;
using Confluent.Kafka;
using MicroServices.Samples.Services.Basket.API.Application.Events;
using MicroServices.Samples.Services.Basket.API.Application.Models;
using MicroServices.Samples.Services.Basket.API.Factory;
using MicroServices.Samples.Shared.Common.Utils;

namespace MicroServices.Samples.Services.Basket.API.BackgroundTasks;

public class ConsumerBackgroundTasks : BackgroundService
{
    private readonly KafkaConsumer<Ignore, string> _kafkaConsumer;
    private readonly ILogger<ConsumerBackgroundTasks> _logger;
    private readonly ICustomerBasketRepositoryFactory _repositoryFactory;

    public ConsumerBackgroundTasks(KafkaConsumer<Ignore, string> kafkaConsumer, ILogger<ConsumerBackgroundTasks> logger, ICustomerBasketRepositoryFactory repositoryFactory)
    {
        _kafkaConsumer = kafkaConsumer;
        _logger = logger;
        _repositoryFactory = repositoryFactory;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var task = new TaskFactory().StartNew(() => _kafkaConsumer.Consume(ConsumerCallBack, "OrderEvents", 0, -1, cancellationTokenSource.Token),
         cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        return task;
    }

    private async void ConsumerCallBack(ConsumeResult<Ignore, string> consumeResult)
    {
        OrderConfirmedIntegrationEvent orderConfirmedIntegrationEvent = JsonSerializer.Deserialize<OrderConfirmedIntegrationEvent>(consumeResult.Message.Value);
        var repository = _repositoryFactory.CreateCustomerBasketRepository();
        CustomerBasket customerBasket = await repository.GetByIdAsync(orderConfirmedIntegrationEvent.IdentityId);
        List<BasketItem> basketItems = new List<BasketItem>();
        basketItems.AddRange(customerBasket.Items);
        foreach (var item in basketItems)
        {
            await repository.DeleteBasketItemAsync(orderConfirmedIntegrationEvent.IdentityId, item.ProductId);
        }
    }
}