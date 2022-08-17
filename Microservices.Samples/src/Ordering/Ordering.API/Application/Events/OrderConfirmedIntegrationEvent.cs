using MicroServices.Samples.Shared.Common.Event;

namespace MicroServices.Samples.Services.Ordering.API.Application.Events;

public class OrderConfirmedIntegrationEvent : IntegrationEvent
{
    public OrderConfirmedIntegrationEvent(string orderId, string identityId) : base()
    {
        OrderId = orderId;
        IdentityId = identityId;
    }
    public string OrderId { get; private set; }
     public string IdentityId { get; private init; }
}