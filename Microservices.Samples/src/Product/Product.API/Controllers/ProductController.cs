using System.Text;
using MicroServices.Samples.Services.Product.API.Application.Models;
using MicroServices.Samples.Services.Product.API.Application.Service;
using MicroServices.Samples.Services.Product.API.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace MicroServices.Samples.Services.Product.API.Controller;
#nullable enable

[ApiController]
[Route("api/Product")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService service, ILogger<ProductController> logger)
    {
        _service = service;
        _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> ListProductItem(string? name, decimal? minPrice, decimal? maxPrice, bool? sortPrice)
    {
        List<ProductItemDTO> listProductItemDTO = new List<ProductItemDTO>();
        var listProductItem = await _service.ListAsync(name, minPrice, maxPrice, sortPrice);
        foreach (var productItem in listProductItem)
        {
            ProductItemDTO productItemDTO = new ProductItemDTO();
            productItemDTO.Id = productItem.Id;
            productItemDTO.Name = productItem.Name;
            productItemDTO.Price = productItem.Price;
            productItemDTO.AvailableQuantity = productItem.AvailableQuantity;
            listProductItemDTO.Add(productItemDTO);
        }
        return Ok(listProductItemDTO);
    }
    [HttpGet("productItem/{id}")]
    public async Task<IActionResult> ProductItemById(int id)
    {
        var productItem = await _service.GetByIdAsync(id);
        return Ok(productItem);
    }
    [HttpPatch]
    [Route("availableQuantity/{id}")]
    public async Task<IActionResult> UpdateAvailableQuantity(int id, ProductItemAvailableQuantityDTO productItemAvailableQuantityDTO)
    {
        if (productItemAvailableQuantityDTO != null)
        {
            var data = await _service.UpdateAvailableQuantityAsync(id, productItemAvailableQuantityDTO.Quantity);
            return Ok(data);
        }
        return BadRequest();
    }
    [HttpPatch]
    [Route("name/{id}")]
    public async Task<IActionResult> UpdateName(int id, ProductItemNameDTO productItemNameDTO)
    {
        if (productItemNameDTO != null)
        {
            var data = await _service.UpdateNameAsync(id, productItemNameDTO.Name);
            return Ok(data);
        }
        return BadRequest();
    }
    [HttpPatch]
    [Route("price/{id}")]
    public async Task<IActionResult> UpdatePrice(int id, ProductItemPriceDTO productItemPriceDTO)
    {
        if (productItemPriceDTO != null)
        {
            var data = await _service.UpdatePriceAsync(id, productItemPriceDTO.Price);
            return Ok(data);
        }
        return BadRequest();
    }
    [HttpPost]
    public async Task<IActionResult> AddProductItem(ProductItemDTO productItemDTO)
    {
        ProductItem productItem = new ProductItem();
        if (productItemDTO != null)
        {
            productItem.Name = productItemDTO.Name;
            productItem.Price = productItemDTO.Price;
            productItem.AvailableQuantity = productItemDTO.AvailableQuantity;
            var data = await _service.AddAsync(productItem);
            return Ok(data);
        }
        return BadRequest();
    }
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteProductItem(int id)
    {
        var data = await _service.DeleteAsync(id);
        return Ok(data);
    }
}