using MicroServices.Samples.Services.Ordering.API.Application.Service;
using MicroServices.Samples.Services.Ordering.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MicroServices.Samples.Services.Ordering.API.Controller;

[ApiController]
[Route("api/Ordering")]
public class OrderingController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly ILogger<OrderingController> _logger;

    public OrderingController(IOrderService service, ILogger<OrderingController> logger)
    {
        _service = service;
        _logger = logger;
    }
    [HttpPost]
    public async Task<IActionResult> AddOrder(UpsertOrder upsertOrder)
    {
        if (upsertOrder != null)
        {
            var data = await _service.AddAsync(upsertOrder);
            return Ok(data);
        }
        return BadRequest();
    }
    [HttpGet("orderById/{orderId}")]
    public async Task<IActionResult> GetOrderById(string orderId)
    {
        var data = await _service.GetByIdAsync(orderId);
        return Ok(data);
    }
    [HttpGet("orderByCustomerId{customerId}")]
    public async Task<IActionResult> GetOrderByCustomerId(string customerId)
    {
        var data = await _service.GetByCustomerIdAsync(customerId);
        return Ok(data);
    }
}