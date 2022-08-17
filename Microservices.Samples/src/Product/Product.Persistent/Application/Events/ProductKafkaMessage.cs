using MicroServices.Samples.Services.Product.ProductPersistent.Application.Model;

namespace MicroServices.Samples.Services.Product.ProductPersistent.Application.Events;

public class ProductKafkaMessage
{
    public ProductKafkaMessage()
    {
    }
    public int ActionType { get; set; }
    public ProductItem Data { get; set; }
    public ProductKafkaMessage(int actionType, ProductItem data)
    {
        ActionType = actionType;
        Data = data;
    }
}