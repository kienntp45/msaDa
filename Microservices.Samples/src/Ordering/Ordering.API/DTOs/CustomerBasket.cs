namespace MicroServices.Samples.Services.Ordering.API.DTOs;

public class CustomerBasket
{
    public string CusTomerId { get; set; }
    public List<BasketItem> Items { get; set; }

    public CustomerBasket()
    {
        Items = new List<BasketItem>();
    }
}