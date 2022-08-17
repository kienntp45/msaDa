using MicroServices.Samples.Services.Basket.API.Application.Models;
using MicroServices.Samples.Services.Basket.API.Application.Service;
using MicroServices.Samples.Services.Basket.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MicroServices.Samples.Services.Basket.API.Controllers;
[ApiController]
[Route("api/customerBasket")]
public class CustomerBasketController : ControllerBase
{
    private readonly ICustomerBasketService _service;
    private readonly ILogger<CustomerBasketController> _logger;
    private static HttpClient _client = new HttpClient();
    private readonly IConfiguration _config;

    public CustomerBasketController(ICustomerBasketService service, ILogger<CustomerBasketController> logger, IConfiguration config)
    {
        _service = service;
        _logger = logger;
        _config = config;
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerBasketById(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            var data = await _service.GetByIdAsync(id);
            return Ok(data);
        }
        return BadRequest();
    }
    [HttpPost]
    public async Task<IActionResult> AddCustomerBasket(UpsertCustomerBasketDTO upsertCustomerBasketDTO)
    {
        var data = await _service.AddAsync(upsertCustomerBasketDTO);
        return Ok(data);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllCustomerBasket()
    {
        List<CustomerBasket> customerBaskets = await _service.ListAsync();
        return Ok(customerBaskets);
    }
}