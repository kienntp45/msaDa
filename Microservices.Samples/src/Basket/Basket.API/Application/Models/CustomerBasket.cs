namespace MicroServices.Samples.Services.Basket.API.Application.Models;

public class CustomerBasket
{
    public string CusTomerId { get; set; }
    public List<BasketItem> Items { get; set; }

    public CustomerBasket()
    {
        Items = new List<BasketItem>();
    }
}