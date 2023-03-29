using MediatR;
using SpeedyAir.Application.AggregateRoots.Flight.Models;
using SpeedyAir.Application.AggregateRoots.Order.Commands;
using SpeedyAir.Application.Exceptions;
using SpeedyAir.Domain;

namespace SpeedyAir.Application.AggregateRoots.Flight.Commands;

public class LoadFlightScheduleCommandHandler : IRequestHandler<LoadFlightScheduleCommand, List<GetFlightViewModel>>
{
    private readonly IFlightRepository _flightRepository;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IMediator _mediator;

    public LoadFlightScheduleCommandHandler(IFlightRepository flightRepository,
        IOrdersRepository ordersRepository,
        IMediator mediator)
    {
        _flightRepository = flightRepository;
        _ordersRepository = ordersRepository;
        _mediator = mediator;
    }
    
    public async Task<List<GetFlightViewModel>> Handle(LoadFlightScheduleCommand request,
        CancellationToken cancellationToken)
    {
        if (request.ScheduleDays.Count <= 0)
        {
            throw new ApplicationLogicException("Schedule is empty");
        }        
        
        var domainFlights = request.ScheduleDays.SelectMany(scheduleDay =>
            scheduleDay.Flights.Select(requestFlight => new Domain.Flight(
                requestFlight.FlightNumber,
                requestFlight.OriginCity,
                requestFlight.OriginAirportCode,
                requestFlight.DestinationCity,
                requestFlight.DestinationAirportCode,
                scheduleDay.DayIndex
            ))).ToList();

        await _flightRepository.AddFlights(domainFlights);
        
        await _flightRepository.SaveChangesAsync(cancellationToken);

        if (request.SchedulePendingOrders)
        {
            await SchedulePendingOrders(domainFlights.Select(x => x.Id).ToList());
        }

        return domainFlights.Select(domainFlight => new GetFlightViewModel()
        {
            DestinationAirportCode = domainFlight.DestinationAirportCode,
            DestinationCity = domainFlight.DestinationCity,
            FlightNumber = domainFlight.FlightNumber,
            OriginCity = domainFlight.OriginCity,
            OriginAirportCode = domainFlight.OriginAirportCode,
            DayIndex = domainFlight.DepartureDay
        }).ToList();
    }

    //TODO: Move to separate service and make a background task, make integration event
    private async Task SchedulePendingOrders(List<int> flightIds)
    {
        var pendingOrders = await _ordersRepository.GetPendingOrders();

        await _mediator.Send(new ScheduleOrdersCommand()
        {
            OrderIds = pendingOrders.Select(x => x.Id).ToList(),
            FlightIds = flightIds
        });
    }
}
