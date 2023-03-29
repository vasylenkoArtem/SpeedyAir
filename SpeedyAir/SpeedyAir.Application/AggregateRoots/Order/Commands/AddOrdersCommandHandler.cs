using MediatR;
using SpeedyAir.Domain;

namespace SpeedyAir.Application.AggregateRoots.Order.Commands;

public class AddOrdersCommandHandler : IRequestHandler<AddOrdersCommand, List<int>>
{
    private readonly IOrdersRepository _ordersRepository;

    public AddOrdersCommandHandler(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task<List<int>> Handle(AddOrdersCommand request, CancellationToken cancellationToken)
    {
        if (request.Orders.Count <= 0)
        {
            throw new Exception("Orders is empty");
        }   
        
        var domainOrders = request.Orders.Select(requestOrder => new Domain.Order(
            requestOrder.OrderIdentificator,
            requestOrder.DestinationAirportCode,
            requestOrder.OriginAirportCode
        )).ToList();

        await _ordersRepository.AddOrders(domainOrders);
        
        await _ordersRepository.SaveChangesAsync(cancellationToken);

        return domainOrders.Select(x => x.Id).ToList();
    }
}
