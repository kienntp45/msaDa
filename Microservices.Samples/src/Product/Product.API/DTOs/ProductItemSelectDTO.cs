namespace MicroServices.Samples.Services.Product.API.DTOs;
#nullable enable


public class ProductItemSelectDTO
{
    public string? Name {get;set;}
    public decimal? MinPrice {get;set;}
    public decimal? MaxPrice{get;set;}
    public bool? sortPrice {get;set;}
}