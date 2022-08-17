namespace MicroServices.Samples.Services.Product.API.Application.Models;
public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int AvailableQuantity { get; set; }

}