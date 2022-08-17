using MicroServices.Samples.Services.Basket.API.Application.Models;
using MicroServices.Samples.Services.Basket.API.Application.Repository;
using MicroServices.Samples.Services.Basket.API.DTOs;
using MicroServices.Samples.Services.Product.API.Application.Models;

namespace MicroServices.Samples.Services.Basket.API.Application.Service;

public class CustomerBasketService : ICustomerBasketService
{
    private readonly ICustomerBasketRepository _repository;
    private readonly ILogger<CustomerBasketService> _logger;
    private readonly IConfiguration _config;
    private readonly HttpClient _client;

    public CustomerBasketService(ICustomerBasketRepository repository, ILogger<CustomerBasketService> logger, IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient();
        _repository = repository;
        _logger = logger;
        _config = config;

    }
    public async Task<UpsertCustomerBasketResponseDTO> AddAsync(UpsertCustomerBasketDTO upsertCustomerBasketDTO)
    {
        string ApiGetProductById = _config["HttpGetProduct"] + "/" + upsertCustomerBasketDTO.ProductId;
        CustomerBasket customerBasket = new CustomerBasket();
        UpsertCustomerBasketResponseDTO upsertCustomerBasketResponseDTO = new UpsertCustomerBasketResponseDTO("", customerBasket);
        HttpResponseMessage response = new HttpResponseMessage();

        response = await _client.GetAsync(ApiGetProductById);
        if (response.IsSuccessStatusCode)
        {
            if (response.Content.Headers.ContentLength != 0)
            {
                var product = await response.Content.ReadFromJsonAsync<ProductDTO>();
                if (product.AvailableQuantity < upsertCustomerBasketDTO.Quantity)
                {
                    return null;
                }
                else
                {
                    var customerBasket1 = await _repository.GetByIdAsync(upsertCustomerBasketDTO.CustomerId);
                    BasketItem basketItem = new BasketItem();
                    int remainQuantity = product.AvailableQuantity - upsertCustomerBasketDTO.Quantity;
                    customerBasket.CusTomerId = upsertCustomerBasketDTO.CustomerId;
                    basketItem.ProductId = upsertCustomerBasketDTO.ProductId;
                    basketItem.ProductName = product.Name;
                    basketItem.Quantity = upsertCustomerBasketDTO.Quantity;
                    basketItem.Status = 1;
                    customerBasket.Items.Add(basketItem);
                    if (customerBasket1 != null)
                    {
                        if (customerBasket1.Items.Any(e => e.ProductId == upsertCustomerBasketDTO.ProductId))
                        {
                            for (int i = 0; i < customerBasket1.Items.Count(); i++)
                            {
                                if (customerBasket1.Items[i].ProductId == upsertCustomerBasketDTO.ProductId)
                                {
                                    if (upsertCustomerBasketDTO.Quantity == 0)
                                    {
                                        upsertCustomerBasketResponseDTO.Data = await _repository.DeleteBasketItemAsync(upsertCustomerBasketDTO.CustomerId, upsertCustomerBasketDTO.ProductId);
                                        upsertCustomerBasketResponseDTO.Message = "Xoa BasketItem";
                                    }
                                    else
                                    {
                                        upsertCustomerBasketResponseDTO.Data = await _repository.UpdateQuantityAsync(upsertCustomerBasketDTO.CustomerId, upsertCustomerBasketDTO.Quantity, upsertCustomerBasketDTO.ProductId);
                                        upsertCustomerBasketResponseDTO.Message = "Cap Nhat BasketItem";
                                    }
                                }
                            }
                        }
                        else
                        {
                            upsertCustomerBasketResponseDTO.Data = await _repository.AddBasketItemAsync(customerBasket);
                            upsertCustomerBasketResponseDTO.Message = "Them moi BasketItem";
                        }
                    }
                    else
                    {
                        upsertCustomerBasketResponseDTO.Data = await _repository.AddAsync(customerBasket);
                        upsertCustomerBasketResponseDTO.Message = "Them moi BasketCustomer";
                    }
                }
                return upsertCustomerBasketResponseDTO;
            }
        }
        upsertCustomerBasketResponseDTO.Data= null;
        upsertCustomerBasketResponseDTO.Message = "Them moi thất bại";
        return upsertCustomerBasketResponseDTO;
    }
    public async Task<CustomerBasket> GetByIdAsync(string customerId)
    {
        return await _repository.GetByIdAsync(customerId);
    }

    public Task<List<CustomerBasket>> ListAsync()
    {
        return _repository.ListAsync();
    }

    public async Task<UpsertCustomerBasketResponseDTO> UpdateStatusAsync(UpsertStatusDTO UpsertStatusDTO)
    {
        CustomerBasket customerBasket = await _repository.UpdateStatusAsync(UpsertStatusDTO.CustomerId, UpsertStatusDTO.Status);
        return new UpsertCustomerBasketResponseDTO("Cập nhật thành công", customerBasket);
    }
}