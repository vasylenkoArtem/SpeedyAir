using MediatR;
using SpeedyAir.Application.AggregateRoots.Order.Commands;
using SpeedyAir.Application.AggregateRoots.Order.Models;

namespace SpeedyAir.ConsoleApp.Services;

public class OrdersLoadingConsoleService : IOrdersLoadingConsoleService
{
    private readonly IMediator _mediator;

    public OrdersLoadingConsoleService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task LoadOrders()
    {
        //todo: replace mock with console
        var inputOrders = new List<AddOrderViewModel>()
        {
            new AddOrderViewModel()
            {
                OrderIdentificator = "dd1",
                DestinationAirportCode = "LHR",
                OriginAirportCode = "FRA"
            }
        };

        var orderIds = await _mediator.Send(new AddOrdersCommand()
        {
            Orders = inputOrders
        });

        var orderViewModels = await _mediator.Send(new ScheduleOrdersCommand()
        {
            OrderIds = orderIds
        });
    }
}