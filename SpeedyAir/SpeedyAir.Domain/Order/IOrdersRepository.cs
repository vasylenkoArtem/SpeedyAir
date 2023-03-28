using SpeedyAir.Infrastructure;

namespace SpeedyAir.Domain;

public interface IOrdersRepository : IRepositoryBase
{
    Task AddOrders(List<Order> orders);

    Task<List<Order>> GetOrders(List<int> orderIds);
}