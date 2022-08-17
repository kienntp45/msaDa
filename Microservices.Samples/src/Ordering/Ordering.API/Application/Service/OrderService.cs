using System.Text.Json;
using Confluent.Kafka;
using MicroServices.Samples.Services.Ordering.API.Application.Commands;
using MicroServices.Samples.Services.Ordering.API.Application.Events;
using MicroServices.Samples.Services.Ordering.API.Application.Models;
using MicroServices.Samples.Services.Ordering.API.Application.Repository;
using MicroServices.Samples.Services.Ordering.API.DTOs;
using MicroServices.Samples.Shared.Common.Event;
using MicroServices.Samples.Shared.Common.Utils;

namespace MicroServices.Samples.Services.Ordering.API.Application.Service;


public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly ILogger<OrderService> _logger;
    private readonly IConfiguration _config;
    private readonly HttpClient _client;
    private readonly KafkaProducer<Null, string> _kafkaProducer;
    private readonly KafkaConsumer<Ignore, string> _kafkaConsumer;
    private readonly InMemoryRequestManagement _requestManagement;
    private readonly ICustomerService _customerService;
    private readonly NetMQPush _netMQPush;

    public OrderService(IOrderRepository repository, ILogger<OrderService> logger, IConfiguration config,
    IHttpClientFactory httpClientFactory, KafkaProducer<Null, string> kafkaProducer, KafkaConsumer<Ignore, string> kafkaConsumer,
    InMemoryRequestManagement requestManagement, ICustomerService customerService,
    NetMQPush netMQPush)
    {
        _repository = repository;
        _logger = logger;
        _config = config;
        _client = httpClientFactory.CreateClient();
        _kafkaProducer = kafkaProducer;
        _kafkaConsumer = kafkaConsumer;
        _requestManagement = requestManagement;
        _customerService = customerService;
        _netMQPush = netMQPush;
    }
    public async Task<UpsertOrderResponse> AddAsync(UpsertOrder upsertOrder)
    {
        string ApiGetCustomerBasketById = _config["HttpGetCustomerBasket"] + "/" + upsertOrder.IdentityId;
        HttpResponseMessage response = new HttpResponseMessage();
        Order order = new Order();
        UpsertOrderResponse upsertOrderResponse = new UpsertOrderResponse("", order);
        List<ProductUpdateQuantity> productUpdateQuantities = new List<ProductUpdateQuantity>();

        response = await _client.GetAsync(ApiGetCustomerBasketById);
        if (response.IsSuccessStatusCode)
        {
            if (response.Content.Headers.ContentLength != 0)
            {
                var customerBasket = await response.Content.ReadFromJsonAsync<CustomerBasket>();
                if (customerBasket.Items.Count() != 0)
                {
                    order.OrderDate = DateTime.Now;
                    order.Street = upsertOrder.Street;
                    order.District = upsertOrder.District;
                    order.City = upsertOrder.City;
                    order.AdditionalAddress = upsertOrder.AdditionalAddress;
                    foreach (var item in customerBasket.Items)
                    {

                        if (item.Status == 1)
                        {
                            var productUpdateQuantity = new ProductUpdateQuantity();
                            OrderItem orderItem = new OrderItem();
                            orderItem.ProductId = item.ProductId;
                            orderItem.ProductName = item.ProductName;
                            orderItem.Quantity = item.Quantity;

                            order.Items.Add(orderItem);
                            productUpdateQuantity.ProductId = item.ProductId;
                            productUpdateQuantity.Quantity = item.Quantity;
                            productUpdateQuantities.Add(productUpdateQuantity);
                        }
                    }
                    var start = DateTime.Now.Ticks;
                    var orderStartedEvent = new ProductUpdateQuantityCommand(productUpdateQuantities, start);
                    _requestManagement.SetRequest(orderStartedEvent.Id);
                    // ProduceEvent(orderStartedEvent);
                    var jsonOrderStartedEvent = JsonSerializer.Serialize(orderStartedEvent);
                    _netMQPush.SendMessage(jsonOrderStartedEvent);
                    var resp = await _requestManagement.GetResponse(orderStartedEvent.Id);
                    var timeTicks = (ProductUpdateQuantityResponseCommand)resp;
                    var duration = DateTime.Now.Ticks - timeTicks.TimeTick;
                    Console.Write($"Add customer in {duration} tick");
                    if (resp != null)
                    {
                        var updateResult = (ProductUpdateQuantityResponseCommand)resp;
                        if (updateResult.Items != null)
                        {
                            Customer customer = new Customer();
                            Customer customer1 = new Customer();
                            customer.IdentityId = upsertOrder.IdentityId;
                            customer.Name = upsertOrder.CustomerName;
                            customer.PhoneNumber = upsertOrder.PhoneNumber;

                            customer1 = await _customerService.AddAsync(customer);
                            order.CustomerId = customer.Id;

                            var orderResult = await _repository.AddAsync(order);
                            if (orderResult != null)
                            {

                                upsertOrderResponse.Data = orderResult;
                                var orderConfirmedIntegrationEvent = new OrderConfirmedIntegrationEvent(orderResult.Id, upsertOrder.IdentityId);
                                ProduceBasketEvent(orderConfirmedIntegrationEvent);
                                upsertOrderResponse.Message = "Thêm mới thành công";
                                return upsertOrderResponse;
                            }
                            foreach (var productUpdateQuantity in productUpdateQuantities)
                            {
                                productUpdateQuantity.Quantity = -productUpdateQuantity.Quantity;
                                ProduceEvent(orderStartedEvent);
                            }
                        }

                    }
                }

            }
        }
        upsertOrderResponse.Message = "Thêm thất bại";
        upsertOrderResponse.Data = null;
        return upsertOrderResponse;
    }

    public Task<UpsertOrderResponse> DeleteAsync(string orderId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Order>> GetByCustomerIdAsync(string customerId)
    {
        return await _repository.GetByCustomerIdAsync(customerId);
    }

    public async Task<Order> GetByIdAsync(string orderId)
    {
        return await _repository.GetByIdAsync(orderId);
    }
    private void ProduceEvent(IntegrationEvent @event)
    {
        var json = JsonSerializer.Serialize(@event, @event.GetType(), new JsonSerializerOptions
        {
            WriteIndented = true
        });
        var message = new Message<Null, string> { Value = json };
        _kafkaProducer.Produce(message, "ProductCommand");
    }
    private void ProduceBasketEvent(IntegrationEvent @event)
    {
        var json = JsonSerializer.Serialize(@event, @event.GetType(), new JsonSerializerOptions
        {
            WriteIndented = true
        });
        var message = new Message<Null, string> { Value = json };
        _kafkaProducer.Produce(message, "OrderEvents");
    }


}