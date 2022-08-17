namespace MicroServices.Samples.Services.Product.API.Application.Models;

public class OrderItem
{
    public int Id {get;set;}
    public string ProductName {get;set;}
    public int ProductId{get;set;}
    public int Quantity {get;set;}
}