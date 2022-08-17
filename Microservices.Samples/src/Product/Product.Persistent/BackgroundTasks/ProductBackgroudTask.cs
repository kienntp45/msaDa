using System.Text.Json;
using Confluent.Kafka;
using Disruptor;
using Disruptor.Dsl;
using MicroServices.Samples.Services.Product.ProductPersistent.Application.Events;
using MicroServices.Samples.Services.Product.ProductPersistent.Application.Model;
using MicroServices.Samples.Services.Product.ProductPersistent.Disruptor;
using MicroServices.Samples.Services.Product.ProductPersistent.Factory;
using MicroServices.Samples.Shared.Common.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MicroServices.Samples.Services.Product.ProductPersistent.BackgroundTasks;


public class ProductBackgroundTask : BackgroundService
{
    private readonly KafkaConsumer<string, string> _kafkaConsumer;
    private readonly ILogger<ProductBackgroundTask> _logger;
    private readonly ProductServiceFactory _factory;
    private readonly RingBuffer<ProductRingMessage> _ringBuffer;
    private readonly int _handlerCount;
    public List<ProductItem> productItems = new List<ProductItem>();
    public int count = 1;
    public Dictionary<string, int> idHandleMessages = new Dictionary<string, int>();
    public ProductBackgroundTask(KafkaConsumer<string, string> kafkaConsumer, ILogger<ProductBackgroundTask> logger, ProductServiceFactory factory,
    ProductKafkaMessageRingBuffer ringBuffer,IConfiguration config)
    {
        _kafkaConsumer = kafkaConsumer;
        _logger = logger;
        _factory = factory;
        _ringBuffer = ringBuffer.CreateRingBuffer();
        _handlerCount = Int32.Parse(config["DisruptorConfig:HandlerCount"]);
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("StartTime 113131 : " + DateTime.Now.ToString("HH:mm:ss.fff"));
        var cancellationToken = new CancellationTokenSource();
        var task = new TaskFactory().StartNew(() => _kafkaConsumer.Consume(ConsumerCallBack, "Test", 0, 0, cancellationToken.Token),
        cancellationToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        return task;
    }
    private void ConsumerCallBack(ConsumeResult<string, string> message)
    {
        if (!idHandleMessages.ContainsKey(message.Message.Key))
            idHandleMessages.Add(message.Message.Key, count);
        count = idHandleMessages[message.Message.Key];
        var sequence = _ringBuffer.Next();
        var data = _ringBuffer[sequence];
        data.Message = message.Message.Value;
        data.IdHandler = count;
        _ringBuffer.Publish(sequence);
        count++;
        if (count == _handlerCount)
        {
            count = 1;
        }
    }

}
