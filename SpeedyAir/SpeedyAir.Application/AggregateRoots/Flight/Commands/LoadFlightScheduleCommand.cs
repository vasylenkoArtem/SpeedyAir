using MediatR;
using SpeedyAir.Application.AggregateRoots.Flight.Models;

namespace SpeedyAir.Application.AggregateRoots.Flight.Commands;

public class LoadFlightScheduleCommand : IRequest<List<FlightViewModel>>
{
    public List<ScheduleDayViewModel> ScheduleDays { get; set; }

    public bool SchedulePendingOrders { get; set; }
}
