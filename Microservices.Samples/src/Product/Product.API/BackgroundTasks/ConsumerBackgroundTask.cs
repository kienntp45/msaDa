using System.Text.Json;
using Confluent.Kafka;
using MicroServices.Samples.Services.Product.API.Application.Commands;
using MicroServices.Samples.Services.Product.API.DTOs;
using MicroServices.Samples.Services.Product.API.Factory;
using MicroServices.Samples.Shared.Common.Utils;
using NetMQ;
using NetMQ.Sockets;

namespace MicroServices.Samples.Services.Product.API.BackgroundTasks;

public class ConsumerBackgroundTask : BackgroundService
{
    private readonly KafkaConsumer<Ignore, string> _kafkaConsumer;
    private readonly KafkaProducer<Null, string> _kafkaProducer;
    private readonly ILogger<ConsumerBackgroundTask> _logger;
    private readonly HttpClient _client;
    private readonly ProductServiceFactory _productServiceFactory;
    private readonly NetMQPush _netMQPush;
    private readonly NetMQPull _netMQPull;

    public ConsumerBackgroundTask(KafkaConsumer<Ignore, string> kafkaConsumer, ILogger<ConsumerBackgroundTask> logger,
     IHttpClientFactory httpClientFactory, ProductServiceFactory productServiceFactory, KafkaProducer<Null, string> kafkaProducer
     , NetMQPush netMQPush,NetMQPull netMQPull)
    {
        _kafkaConsumer = kafkaConsumer;
        _kafkaProducer = kafkaProducer;
        _logger = logger;
        _client = httpClientFactory.CreateClient();
        _productServiceFactory = productServiceFactory;
        _netMQPush = netMQPush;
        _netMQPull = netMQPull;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        // var task = new TaskFactory().StartNew(() => _kafkaConsumer.Consume(ConsumeCallback, "ProductCommand", 0, -1, cancellationTokenSource.Token)
        // , cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        var task = new TaskFactory().StartNew(() =>
            _netMQPull.ReceiveMessage(callback, cancellationTokenSource.Token), cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        return task;
    }
    public async void callback(string message)
    {
        ProductUpdateQuantityCommand productUpdateQuantityCommand = JsonSerializer.Deserialize<ProductUpdateQuantityCommand>(message);
        var duration = DateTime.Now.Ticks - productUpdateQuantityCommand.TimeTick;
        Console.WriteLine("time send kafka: "+duration);
        List<ProductItemAvailableQuantityDTO> ProductItems = new List<ProductItemAvailableQuantityDTO>();
        string newMessage = "";
        var _productService = _productServiceFactory.CreateProductService();
        foreach (var item in productUpdateQuantityCommand.Items)
        {
            var product = _productService.GetByIdAsync(item.ProductId).Result;
            if (product != null)
            {
                if (product.AvailableQuantity >= item.Quantity)
                {
                    ProductItems.Add(item);
                }
            }
        }
        if (ProductItems.Count() == productUpdateQuantityCommand.Items.Count())
        {
            foreach (var item in ProductItems)
            {
                var product = _productService.GetByIdAsync(item.ProductId).Result;
                int quantity = product.AvailableQuantity - item.Quantity;
                await _productService.UpdateAvailableQuantityAsync(item.ProductId, quantity);
            }
            long timeTick = DateTime.Now.Ticks;
            var ProductUpdateQuantityResponseCommand = new ProductUpdateQuantityResponseCommand(productUpdateQuantityCommand.Id, productUpdateQuantityCommand.Items,timeTick);
            var json = JsonSerializer.Serialize(ProductUpdateQuantityResponseCommand);
            newMessage = json;
        }
        else
        {
            long timeTick = DateTime.Now.Ticks;
            var ProductUpdateQuantityResponseCommand = new ProductUpdateQuantityResponseCommand(productUpdateQuantityCommand.Id, new List<ProductItemAvailableQuantityDTO>(),timeTick);
            var json = JsonSerializer.Serialize(ProductUpdateQuantityResponseCommand);
            newMessage = json;
        }
        _netMQPush.SendMessage(message);
    }
    // private async void ConsumeCallback(ConsumeResult<Ignore, string> consumeResult)
    // {
    //     ProductUpdateQuantityCommand productUpdateQuantityCommand = JsonSerializer.Deserialize<ProductUpdateQuantityCommand>(consumeResult.Message.Value);
    //     var duration = DateTime.Now.Ticks - productUpdateQuantityCommand.TimeTick;
    //     Console.WriteLine("time send kafka: "+duration);
    //     List<ProductItemAvailableQuantityDTO> ProductItems = new List<ProductItemAvailableQuantityDTO>();
    //     string message = "";
    //     var _productService = _productServiceFactory.CreateProductService();
    //     foreach (var item in productUpdateQuantityCommand.Items)
    //     {
    //         var product = _productService.GetByIdAsync(item.ProductId).Result;
    //         if (product != null)
    //         {
    //             if (product.AvailableQuantity >= item.Quantity)
    //             {
    //                 ProductItems.Add(item);
    //             }
    //         }
    //     }
    //     if (ProductItems.Count() == productUpdateQuantityCommand.Items.Count())
    //     {
    //         foreach (var item in ProductItems)
    //         {
    //             var product = _productService.GetByIdAsync(item.ProductId).Result;
    //             int quantity = product.AvailableQuantity - item.Quantity;
    //             await _productService.UpdateAvailableQuantityAsync(item.ProductId, quantity);
    //         }
    //         long timeTick = DateTime.Now.Ticks;
    //         var ProductUpdateQuantityResponseCommand = new ProductUpdateQuantityResponseCommand(productUpdateQuantityCommand.Id, productUpdateQuantityCommand.Items,timeTick);
    //         var json = JsonSerializer.Serialize(ProductUpdateQuantityResponseCommand);
    //         message = json;
    //     }
    //     else
    //     {
    //         long timeTick = DateTime.Now.Ticks;
    //         var ProductUpdateQuantityResponseCommand = new ProductUpdateQuantityResponseCommand(productUpdateQuantityCommand.Id, new List<ProductItemAvailableQuantityDTO>(),timeTick);
    //         var json = JsonSerializer.Serialize(ProductUpdateQuantityResponseCommand);
    //         message = json;
    //     }
    //     _netMQPush.SendMessage(message);
    //     // _kafkaProducer.Produce(message, "ProductEvents");
    // }
}