namespace MicroServices.Samples.Services.Product.ProductPersistent.Application.Model;
public class ProductItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int AvailableQuantity { get; set; }

}