namespace MicroServices.Samples.Services.Basket.API.DTOs;

public class UpsertCustomerBasketResponseDTO
{
    public string Message { get; set; }
    public object Data { get; set; }
    public UpsertCustomerBasketResponseDTO(string message, object data)
    {
        Message = message;
        Data = data;
    }
}