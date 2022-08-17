using MicroServices.Samples.Services.Product.ProductPersistent.Application.Model;
#nullable enable
namespace MicroServices.Samples.Services.Product.ProductPersistent.Application.Repository;


public interface IProductRepository
{
    Task<List<ProductItem>> AddAsync(List<ProductItem> productItems);
    Task<ProductItem> GetByIdAsync(int id);
    Task<ProductItem> UpdateNameAsync(int id, string name);
    Task<ProductItem> UpdateAvailableQuantityAsync(int id, int quantity);
    Task<ProductItem> UpdatePriceAsync(int id, decimal price);
    Task<List<ProductItem>> DeleteAsync(List<ProductItem> productItems);
    Task<List<ProductItem>> UpdateAsync(List<ProductItem> productItems);
    Task<int> SaveChangesAsync();
}