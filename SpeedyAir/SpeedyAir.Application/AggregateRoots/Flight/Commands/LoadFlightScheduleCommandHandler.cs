using MediatR;
using SpeedyAir.Application.AggregateRoots.Flight.Models;

namespace SpeedyAir.Application.AggregateRoots.Flight.Commands;

public class LoadFlightScheduleCommandHandler : IRequestHandler<LoadFlightScheduleCommand, List<FlightViewModel>>
{
    public async Task<List<FlightViewModel>> Handle(LoadFlightScheduleCommand request,
        CancellationToken cancellationToken)
    {
        // add flights
        
        //right before return success, get pending orders and call ScheduleOrdersCommand using ids
        throw new NotImplementedException();
    }
}
