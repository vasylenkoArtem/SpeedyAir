using Microsoft.EntityFrameworkCore;
using SpeedyAir.Domain;
using SpeedyAir.Domain.Exceptions;

namespace SpeedyAir.Infrastructure.Aggregates.Order;

public class OrdersRepository : RepositoryBase, IOrdersRepository
{
    public OrdersRepository(SpeedyAirDbContext dbContext) : base(dbContext)
    {
    }

    public async Task AddOrders(List<Domain.Order> orders)
    {
        var orderIdentifiers = orders.Select(x => x.OrderIdentifier);

        var existingOrder = await _dbContext.Orders.FirstOrDefaultAsync(
            x => orderIdentifiers.Contains(x.OrderIdentifier));

        if (existingOrder != null)
        {
            throw new DomainException($"Order with identifier {existingOrder.OrderIdentifier} already exists");
        }

        await _dbContext.Orders.AddRangeAsync(orders);
    }

    public async Task<List<Domain.Order>> GetOrders(List<int> orderIds)
    {
        return await _dbContext.Orders.Where(x => orderIds.Contains(x.Id))
            .ToListAsync();
    }
}