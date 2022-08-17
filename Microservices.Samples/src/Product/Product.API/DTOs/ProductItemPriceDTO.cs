using System.ComponentModel.DataAnnotations;

namespace MicroServices.Samples.Services.Product.API.DTOs;
public class ProductItemPriceDTO
{
    public int Id { get; set; }
    [Range(1000.00,100000.00,ErrorMessage ="Price must be between 1000.00 and 100000.00")]
    public decimal Price { get; set; }

}