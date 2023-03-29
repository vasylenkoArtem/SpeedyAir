using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpeedyAir.Application.AggregateRoots.Order.Commands;
using SpeedyAir.Application.AggregateRoots.Order.Models;
using SpeedyAir.ConsoleApp.Exceptions;
using SpeedyAir.ConsoleApp.Services.Extensions;

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
        Console.WriteLine(
            "Please specify path to the json file with orders: ");

        var ordersJsonFilePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(ordersJsonFilePath))
        {
            ConsoleServiceExtensions.ThrowInputValidationError("File path");
        }

        var inputOrders = new List<AddOrderViewModel>();

        try
        {
            using (StreamReader r = new StreamReader(ordersJsonFilePath))
            {
                string json = await r.ReadToEndAsync();

                var deserializedJson = JsonConvert.DeserializeObject<JObject>(json);
            
                foreach (var property in deserializedJson.Properties())
                {
                    var orderIdentificator = property.Name;
                    var destinationAirportCode = property.Value.First().Values().First().Value<string>();
                
                    inputOrders.Add(new AddOrderViewModel()
                    {
                        OrderIdentificator = orderIdentificator,
                        DestinationAirportCode = destinationAirportCode,
                        OriginAirportCode = "YUL"
                    });
                }

            }
        }
        catch (Exception ex)
        {
            throw new ConsoleAppLogicException($"Error happened during parsing json: message: {ex.Message}");
        }

        var orderIds = await _mediator.Send(new AddOrdersCommand()
        {
            Orders = inputOrders
        });

        var orderViewModels = await _mediator.Send(new ScheduleOrdersCommand()
        {
            OrderIds = orderIds
        });

        foreach (var order in orderViewModels)
        {
            var orderIdentifierText = $"order: {order.OrderIdentifier}, ";

            if (!order.FlightNumber.HasValue)
            {
                Console.WriteLine(orderIdentifierText + "not scheduled");
            }
            else
            {
                Console.WriteLine(orderIdentifierText +
                                  $"flightNumber: {order.FlightNumber}, departure: {order.DepartureCity}, arrival: {order.ArrivalCity}, day: {order.DayIndex}");
            }
        }
    }

    public class InputOrder
    {
        public string Destination { get; set; }
    }
}