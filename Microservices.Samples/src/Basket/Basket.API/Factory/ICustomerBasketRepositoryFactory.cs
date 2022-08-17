using MicroServices.Samples.Services.Basket.API.Repository;

namespace MicroServices.Samples.Services.Basket.API.Factory;


public interface ICustomerBasketRepositoryFactory
{
    CustomerBasketRepository CreateCustomerBasketRepository();
}