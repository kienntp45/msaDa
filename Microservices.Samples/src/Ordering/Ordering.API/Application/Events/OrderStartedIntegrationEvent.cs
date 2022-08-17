using MicroServices.Samples.Services.Ordering.API.DTOs;
using MicroServices.Samples.Shared.Common.Event;

namespace MicroServices.Samples.Services.Ordering.API.Application.Events;

public class OrderStartedIntegrationEvent : IntegrationEvent
{
    public OrderStartedIntegrationEvent(string customerId, List<ProductUpdateQuantity> items) : base()
    {
        CustomerId = customerId;
        Items = items;
    }
    public string CustomerId { get; private init; }
    public List<ProductUpdateQuantity> Items { get; private set; }
}