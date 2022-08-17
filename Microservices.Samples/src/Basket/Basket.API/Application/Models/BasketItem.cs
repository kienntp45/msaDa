namespace MicroServices.Samples.Services.Basket.API.Application.Models;

public class BasketItem
{
    public string Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public int Status { get; set; }
}