using Disruptor;
using Disruptor.Dsl;
using MicroServices.Samples.Services.Product.ProductPersistent.Application.Events;
using MicroServices.Samples.Services.Product.ProductPersistent.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MicroServices.Samples.Services.Product.ProductPersistent.Disruptor;

public class ProductKafkaMessageRingBuffer
{
    private readonly IDbContextFactory<ProductDbContext> _dbContextFactory;
    private readonly int _handlerCount;
    private readonly int _bufferSize;

    public ProductKafkaMessageRingBuffer(IDbContextFactory<ProductDbContext> dbContextFactory,IConfiguration config)
    {
        _dbContextFactory = dbContextFactory;
        _handlerCount = Int32.Parse(config["DisruptorConfig:HandlerCount"]);
        _bufferSize =Int32.Parse(config["DisruptorConfig:BufferSize"]);
    }
    public RingBuffer<ProductRingMessage> CreateRingBuffer()
    {
        var list = new List<ProductKafkaMessageHandler>();
        for (int i = 0; i < _handlerCount; i++)
        {
            list.Add(new ProductKafkaMessageHandler(_dbContextFactory, i + 1));
        }
        var disruptor = new Disruptor<ProductRingMessage>(() => new ProductRingMessage(), _bufferSize);
        disruptor.HandleEventsWith(list.ToArray());
        return disruptor.Start();
    }
}