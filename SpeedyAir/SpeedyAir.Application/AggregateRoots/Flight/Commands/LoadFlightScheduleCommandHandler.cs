using MediatR;
using SpeedyAir.Application.AggregateRoots.Flight.Models;
using SpeedyAir.Domain;
using ApplicationException = SpeedyAir.Application.Exceptions.ApplicationException;

namespace SpeedyAir.Application.AggregateRoots.Flight.Commands;

public class LoadFlightScheduleCommandHandler : IRequestHandler<LoadFlightScheduleCommand, List<FlightViewModel>>
{
    private readonly IFlightRepository _flightRepository;

    public LoadFlightScheduleCommandHandler(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }
    
    public async Task<List<FlightViewModel>> Handle(LoadFlightScheduleCommand request,
        CancellationToken cancellationToken)
    {
        if (request.ScheduleDays.Count <= 0)
        {
            throw new ApplicationException("Schedule is empty");
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

        return domainFlights.Select(domainFlight => new FlightViewModel()
        {
            DestinationAirportCode = domainFlight.DestinationAirportCode,
            DestinationCity = domainFlight.DestinationCity,
            FlightNumber = domainFlight.FlightNumber,
            OriginCity = domainFlight.OriginCity,
            OriginAirportCode = domainFlight.OriginAirportCode
        }).ToList();
    }
}
