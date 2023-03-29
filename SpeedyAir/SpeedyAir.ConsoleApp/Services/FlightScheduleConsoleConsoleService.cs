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
        Console.WriteLine(
            "Please enter flights schedule with data separated by commas in the format ({Day number},{Flight number},{Origin City},{Origin Airport code},{Destination city},{Destination airport code})");

        var inputString = Console.ReadLine();

        var parameters = inputString.Split(',');

        if (parameters.Length != 6)
        {
            ThrowInputValidationErrorWithAction();
        }

        var dayNumber = parameters[0];
        var flightNumber = parameters[1];
        var originCity = parameters[2];
        var originAirportCode = parameters[3];
        var destinationCity = parameters[4];
        var destinationAirportCode = parameters[5];

        var dayNumberConverted = 0;
        var flightNumberConverted = 0;
        
        if (!int.TryParse(dayNumber, out dayNumberConverted))
        {
            ThrowInputValidationErrorWithAction("Day number");
        }
        
        if (!int.TryParse(flightNumber, out flightNumberConverted))
        {
            ThrowInputValidationErrorWithAction("Flight number");
        }
        
        if (string.IsNullOrEmpty(originCity))
        {
            ThrowInputValidationErrorWithAction("Origin city");
        }
        
        if (string.IsNullOrEmpty(originAirportCode))
        {
            ThrowInputValidationErrorWithAction("Origin Airport Code");
        }
        
        if (string.IsNullOrEmpty(destinationCity))
        {
            ThrowInputValidationErrorWithAction("Destination City");
        }
        
        if (string.IsNullOrEmpty(destinationAirportCode))
        {
            ThrowInputValidationErrorWithAction("Destination Airport Code");
        }

        //todo: replace mock with console
        bool schedulePendingOrders = false;

        var scheduleDays = new List<ScheduleDayViewModel>()
        {
            new ScheduleDayViewModel()
            {
                DayIndex = dayNumberConverted,
                Flights = new List<CreateFlightViewModel>()
                {
                    new CreateFlightViewModel()
                    {
                        OriginCity = originCity,
                        OriginAirportCode = originAirportCode,
                        DestinationCity = destinationCity,
                        DestinationAirportCode = destinationAirportCode,
                        FlightNumber = flightNumberConverted
                    }
                }
            }
        };

        var resultFlights = await _mediator.Send(new LoadFlightScheduleCommand()
        {
            SchedulePendingOrders = schedulePendingOrders,
            ScheduleDays = scheduleDays
        });

        var orderedFlights = resultFlights.OrderBy(x => x.DayIndex);

        var dayIndexes = orderedFlights.Select(x => x.DayIndex).Distinct();

        foreach (var dayIndex in dayIndexes)
        {
            Console.WriteLine($"Day {dayIndex}:");

            var dayIndexFlights = orderedFlights.Where(x => x.DayIndex == dayIndex);
            
            foreach (var flight in dayIndexFlights)
            {
                Console.WriteLine($"Flight {flight.FlightNumber}: {flight.OriginCity} ({flight.OriginAirportCode}) to {flight.DestinationCity} ({flight.DestinationAirportCode})");
            }
        }
    }

    private static void ThrowInputValidationErrorWithAction(string fieldName = null)
    {
        var fieldNameText = !string.IsNullOrEmpty(fieldName) ? $" in the {fieldName} field" : string.Empty;
        
        Console.WriteLine($"Wrong input{fieldNameText}, Do you want to try again? (Y/N)");
        
        var input = Console.ReadLine();
        if (input == "Y" || input == "y")
        {
            //restart
        }
        //break;
    }
}