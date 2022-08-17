using MicroServices.Samples.Services.Ordering.API.DTOs;
using MicroServices.Samples.Shared.Common.Event;

namespace MicroServices.Samples.Services.Ordering.API.Application.Commands;

public class ProductUpdateQuantityCommand : IntegrationEvent
{
    public ProductUpdateQuantityCommand(List<ProductUpdateQuantity> items,  long timeTick) : base()
    {
        Items = items;
        TimeTick = timeTick;
    }
    public List<ProductUpdateQuantity> Items { get; private set; }
    public long TimeTick {get;private set; }
}