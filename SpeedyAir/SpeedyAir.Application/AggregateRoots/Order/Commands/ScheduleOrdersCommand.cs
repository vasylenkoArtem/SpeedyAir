using MediatR;
using SpeedyAir.Application.AggregateRoots.Order.Models;

namespace SpeedyAir.Application.AggregateRoots.Order.Commands;

// take saved orders from db by ids and schedule using available flights
public class ScheduleOrdersCommand : IRequest<List<OrderViewModel>>
{
    public List<int> OrderIds { get; set; }

    public List<int> FlightIds { get; set; }
}