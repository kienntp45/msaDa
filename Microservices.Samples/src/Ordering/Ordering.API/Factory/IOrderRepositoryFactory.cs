using MicroServices.Samples.Services.Ordering.API.Application.Repository;

namespace MicroServices.Samples.Services.Ordering.API.Factory;


public interface IOrderRepositoryFactory
{
     IOrderRepository CreateOrderRepository();
}