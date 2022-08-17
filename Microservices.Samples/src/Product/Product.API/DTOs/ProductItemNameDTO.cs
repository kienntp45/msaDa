using System.ComponentModel.DataAnnotations;

namespace MicroServices.Samples.Services.Product.API.DTOs;
public class ProductItemNameDTO
{
    public int Id { get; set; }
    [MinLength(3,ErrorMessage ="Name must be longer than 3 char")]
    [MaxLength(200)]
    public string Name { get; set; }

}