using MicroServices.Samples.Services.Product.API.DTOs;
using MicroServices.Samples.Shared.Common.Event;

namespace MicroServices.Samples.Services.Product.API.Application.Commands;

public class ProductUpdateQuantityResponseCommand : IntegrationEvent
{
    public ProductUpdateQuantityResponseCommand(Guid id, List<ProductItemAvailableQuantityDTO> items,long timeTick) : base(id,DateTime.Now)
    {
        Items = items;
        TimeTick= timeTick;
    }
    public List<ProductItemAvailableQuantityDTO> Items { get; private set; }

    public long TimeTick {get;set;}
}