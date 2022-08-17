using System.Text.Json.Serialization;
using MicroServices.Samples.Services.Product.API.Application.Models;
using MicroServices.Samples.Shared.Common.Event;

namespace MicroServices.Samples.Services.Product.API.Application.Events;

public class ProductQuantityUpdatedIntegrationEvent : IntegrationEvent
{
    public ProductQuantityUpdatedIntegrationEvent(Guid id, bool isSuccess, List<OrderItem> items) : base(id, DateTime.Now)
    {
        IsSuccess = isSuccess;
        Items = items;
    }

    [JsonConstructor]
    public ProductQuantityUpdatedIntegrationEvent(Guid id, DateTime creationDate, bool isSuccess, List<OrderItem> items) : base(id, creationDate)
    {
        IsSuccess = isSuccess;
        Items = items;
    }
    [JsonInclude]
    public bool IsSuccess { get; private init; }
    [JsonInclude]
    public List<OrderItem> Items { get; private set; }
}