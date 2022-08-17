namespace MicroServices.Samples.Services.Product.API.Application.Events;

public class ProductKafkaMessage
{
    public int ActionType {get;set;}
    public object Data {get;set;}
     public ProductKafkaMessage(int actionType, object data)
    {
        ActionType = actionType;
        Data = data;
    }
}