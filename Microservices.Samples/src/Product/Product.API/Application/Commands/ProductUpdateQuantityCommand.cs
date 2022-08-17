using MicroServices.Samples.Services.Product.API.DTOs;
using MicroServices.Samples.Shared.Common.Event;

namespace MicroServices.Samples.Services.Product.API.Application.Commands;

public class ProductUpdateQuantityCommand : IntegrationEvent
{
    public ProductUpdateQuantityCommand(List<ProductItemAvailableQuantityDTO> items,long timeTick) : base()
    {
        Items = items;
        TimeTick = timeTick;
    }
    public List<ProductItemAvailableQuantityDTO> Items { get; private set; }
    public long TimeTick {get; private set;}
}