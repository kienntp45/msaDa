namespace MicroServices.Samples.Services.Ordering.API.Application.Models;


public class Order
{
    public string Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string CustomerId { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string AdditionalAddress { get; set; }
    public List<OrderItem> Items { get; set; }
    public Order()
    {
        Items = new List<OrderItem>();
    }
}