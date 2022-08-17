using MicroServices.Samples.Services.Product.API.Application.Models;

namespace MicroServices.Samples.Services.Product.API.Database;

public class ProductInMemoryContext
{
    public ProductInMemoryContext()
    {
        Products = new Dictionary<int, ProductItem>();
    }
    public Dictionary<int, ProductItem> Products { get; set; }
}