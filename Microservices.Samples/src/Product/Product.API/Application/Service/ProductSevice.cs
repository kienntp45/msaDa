using System.Text.Json;
using Confluent.Kafka;
using MicroServices.Samples.Services.Product.API.Application.Events;
using MicroServices.Samples.Services.Product.API.Application.Models;
using MicroServices.Samples.Services.Product.API.Application.Repository;
using MicroServices.Samples.Shared.Common.Utils;

namespace MicroServices.Samples.Services.Product.API.Application.Service;


public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductService> _logger;
    private readonly KafkaProducer<string, string> _kafkaProducer;

    public ProductService(IProductRepository repository, ILogger<ProductService> logger, KafkaProducer<string, string> kafkaProducer)
    {
        _repository = repository;
        _logger = logger;
        _kafkaProducer = kafkaProducer;
    }
    public async Task<ProductItem> AddAsync(ProductItem productItem)
    {
        return await _repository.AddAsync(productItem);
    }

    public async Task<ProductItem> DeleteAsync(int id)
    {
        var productItem = await _repository.DeleteAsync(id);
        if (productItem != null)
        {
            string productId = productItem.Id.ToString();
            ProductKafkaMessage productKafkaMessage = new ProductKafkaMessage(0, productItem);
            var json = JsonSerializer.Serialize(productKafkaMessage);
            ProduceEvent(productId,json);
        }
        return productItem;
    }

    public async Task<ProductItem> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<ProductItem>> ListAsync(string? name, decimal? minPrice, decimal? maxPrice, bool? sortPrice)
    {
        return await _repository.ListAsync(name, minPrice, maxPrice, sortPrice);
    }
    public async Task<ProductItem> UpdateAvailableQuantityAsync(int id, int quantity)
    {
        var productItem = await _repository.UpdateAvailableQuantityAsync(id, quantity);
        if (productItem != null)
        {
            string productId = productItem.Id.ToString();
            ProductKafkaMessage productKafkaMessage = new ProductKafkaMessage(0, productItem);
            var json = JsonSerializer.Serialize(productKafkaMessage);
            ProduceEvent(productId,json);
        }
        return productItem;
    }

    public async Task<ProductItem> UpdateNameAsync(int id, string name)
    {
        return await _repository.UpdateNameAsync(id, name);
    }

    public async Task<ProductItem> UpdatePriceAsync(int id, decimal price)
    {
        return await _repository.UpdatePriceAsync(id, price);
    }
    private void ProduceEvent(string key, string json)
    {
        var message = new Message<string, string> { Key = key, Value = json };
        _kafkaProducer.Produce(message, "ProductDatabaseTopic");
    }
}