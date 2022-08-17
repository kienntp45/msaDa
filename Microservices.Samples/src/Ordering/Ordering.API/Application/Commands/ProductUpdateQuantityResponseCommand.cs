using MicroServices.Samples.Services.Ordering.API.DTOs;
using MicroServices.Samples.Shared.Common.Event;

namespace MicroServices.Samples.Services.Ordering.API.Application.Commands;

public class ProductUpdateQuantityResponseCommand : IntegrationEvent
{
    public ProductUpdateQuantityResponseCommand(Guid id, List<ProductUpdateQuantity> items,long timeTick) : base(id,DateTime.Now)
    {
        Items = items;
        TimeTick= timeTick;
    }
    public List<ProductUpdateQuantity> Items { get; private set; }
     public long TimeTick {get;set;}
}