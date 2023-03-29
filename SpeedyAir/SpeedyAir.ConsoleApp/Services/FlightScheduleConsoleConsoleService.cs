using MediatR;
using SpeedyAir.Application.AggregateRoots.Flight.Commands;
using SpeedyAir.Application.AggregateRoots.Flight.Models;

namespace SpeedyAir.ConsoleApp.Services;

public class FlightScheduleConsoleConsoleService : IFlightScheduleConsoleService
{
    private readonly IMediator _mediator;

    public FlightScheduleConsoleConsoleService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task LoadFlightSchedule()
    {
        //todo: replace mock with console
        bool schedulePendingOrders = false;

        var scheduleDays = new List<ScheduleDayViewModel>()
        {
            new ScheduleDayViewModel()
            {
                DayIndex = 1,
                Flights = new List<FlightViewModel>()
                {
                    new FlightViewModel()
                    {
                        OriginCity = "Frankfurt",
                        OriginAirportCode = "FRA",
                        DestinationCity = "London",
                        DestinationAirportCode = "LHR",
                        FlightNumber = 4
                    }
                }
            }
        };

        var result = await _mediator.Send(new LoadFlightScheduleCommand()
        {
            SchedulePendingOrders = schedulePendingOrders,
            ScheduleDays = scheduleDays
        });
    }
}
