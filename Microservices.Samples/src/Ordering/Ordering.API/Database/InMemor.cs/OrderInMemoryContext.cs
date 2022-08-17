using MicroServices.Samples.Services.Ordering.API.Application.Models;

namespace  MicroServices.Samples.Services.Ordering.API.Database.InMemory;


public class OrderInMemoryContext 
{
    public  Dictionary<string, Order> Orders {get;set;}
    public OrderInMemoryContext()
    {
        Orders = new Dictionary<string, Order>();
    }
}