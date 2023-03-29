using MediatR;
using SpeedyAir.Application.AggregateRoots.Order.Models;

namespace SpeedyAir.Application.AggregateRoots.Order.Commands;

public class ScheduleOrdersCommand : IRequest<List<OrderViewModel>>
{
    public List<int> OrderIds { get; set; }

    public List<int> FlightIds { get; set; }
}