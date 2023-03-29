using MediatR;
using SpeedyAir.Application.AggregateRoots.Order.Models;
using SpeedyAir.Domain;

namespace SpeedyAir.Application.AggregateRoots.Order.Commands;

public class ScheduleOrdersCommandHandler : IRequestHandler<ScheduleOrdersCommand, List<OrderViewModel>>
{
    private readonly IFlightRepository _flightRepository;
    private readonly IOrdersRepository _ordersRepository;

    public ScheduleOrdersCommandHandler(
        IFlightRepository flightRepository,
        IOrdersRepository ordersRepository)
    {
        _flightRepository = flightRepository;
        _ordersRepository = ordersRepository;
    }

    public async Task<List<OrderViewModel>> Handle(ScheduleOrdersCommand request, CancellationToken cancellationToken)
    {
        var flights = await _flightRepository.GetAvailableFlights(request.FlightIds);

        var orderedFlights = flights.OrderBy(x => x.OriginAirportCode)
            .ThenBy(x => x.DestinationAirportCode)
            .ThenBy(x => x.DepartureDay);

        var originDestinationToFlightListDictionary = new Dictionary<(string, string), List<Domain.Flight>>();

        foreach (var flight in orderedFlights)
        {
            if (originDestinationToFlightListDictionary.TryGetValue((flight.OriginAirportCode,
                    flight.DestinationAirportCode), out var existingDictionaryItem))
            {
                existingDictionaryItem.Add(flight);
            }
            else
            {
                originDestinationToFlightListDictionary.Add((flight.OriginAirportCode,
                    flight.DestinationAirportCode), new List<Domain.Flight>() { flight });
            }
        }

        var domainOrders = await _ordersRepository.GetOrders(request.OrderIds);

        foreach (var order in domainOrders)
        {
            if (order.Flight != null)
            {
                continue;
            }

            if (originDestinationToFlightListDictionary.TryGetValue(
                    (order.OriginAirportCode, order.DestinationAirportCode), out var flightList))
            {
                var selectedFlight = flightList.FirstOrDefault(x => x.Orders == null || x.Orders.Count < x.MaxAmountOfBoxes);

                if (selectedFlight is null)
                {
                    continue;
                }

                selectedFlight.AddOrder(order);
            }
        }

        await _flightRepository.SaveChangesAsync(cancellationToken);

        return domainOrders.Select(domainOrder => new OrderViewModel()
        {
            ArrivalCity = domainOrder.Flight?.DestinationCity,
            DayIndex = domainOrder.Flight?.DepartureDay,
            DepartureCity = domainOrder.Flight?.OriginCity,
            OrderIdentifier = domainOrder.OrderIdentifier,
            FlightNumber = domainOrder.Flight?.FlightNumber
        }).ToList();
    }
}