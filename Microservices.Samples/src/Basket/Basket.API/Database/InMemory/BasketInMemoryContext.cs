using MicroServices.Samples.Services.Basket.API.Application.Models;

namespace MicroServices.Samples.Services.Basket.API.Database.InMemory;

public class BasketInMemoryContext
{
    public Dictionary<string,CustomerBasket> customerBaskets {get;set;}
    public BasketInMemoryContext(){
        customerBaskets = new Dictionary<string, CustomerBasket>();
    }
}