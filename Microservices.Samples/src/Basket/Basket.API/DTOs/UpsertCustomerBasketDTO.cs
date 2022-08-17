namespace MicroServices.Samples.Services.Basket.API.DTOs;


public class UpsertCustomerBasketDTO 
{
    public string CustomerId {get;set;}
    public int ProductId {get;set;}
    public int Quantity {get;set;}
}