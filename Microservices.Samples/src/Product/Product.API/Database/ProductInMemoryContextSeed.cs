using MicroServices.Samples.Services.Product.API.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.Samples.Services.Product.API.Database;

public class ProductInMemoryContextSeed
{
    public async Task SeedAsync(ProductInMemoryContext inMemoryContext, ProductDbContext dbContext)
    {
        var products = await dbContext.Products.ToListAsync();
        foreach(var prod in products)
        {
            inMemoryContext.Products.Add(prod.Id, prod);
        }
    }
}