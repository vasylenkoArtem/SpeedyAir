using FluentAssertions;
using MediatR;
using Moq;
using SpeedyAir.Application.AggregateRoots.Flight.Commands;
using SpeedyAir.Application.AggregateRoots.Flight.Models;
using SpeedyAir.Application.Exceptions;
using SpeedyAir.Domain;

namespace SpeedyAir.UnitTests.Application.Aggregates.Flight.Commands;

public class LoadFlightsScheduleCommandHandlerTests
{
    [Fact]
    public async Task Handler_SetupWithTwoFlights_SaveToDbAndReturnViewModels()
    {
        //Setup
        var firstInputFlight = new CreateFlightViewModel()
        {
            FlightNumber = 11,
            OriginCity = "Montreal",
            OriginAirportCode = "YUL",
            DestinationAirportCode = "YYZ",
            DestinationCity = "Toronto"
        };

        var secondInputFlight = new CreateFlightViewModel()
        {
            FlightNumber = 12,
            OriginCity = "Montreal",
            OriginAirportCode = "YUL",
            DestinationAirportCode = "YYC",
            DestinationCity = "Calgary"
        };

        var dayIndex = 1;

        var command = new LoadFlightScheduleCommand()
        {
            SchedulePendingOrders = false,
            ScheduleDays = new List<ScheduleDayViewModel>()
            {
                new ScheduleDayViewModel()
                {
                    DayIndex = dayIndex,
                    Flights = new List<CreateFlightViewModel>()
                    {
                        firstInputFlight,
                        secondInputFlight
                    }
                }
            }
        };

        var mediatorMock = new Mock<IMediator>();
        var flightRepositoryMock = new Mock<IFlightRepository>();
        var ordersRepositoryMock = new Mock<IOrdersRepository>();
        
        var handler = new LoadFlightScheduleCommandHandler(flightRepositoryMock.Object, ordersRepositoryMock.Object,
            mediatorMock.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assert
        result.Count.Should().Be(2);

        var firstFlight = result.First();
        var secondFlight = result.Last();

        // Day index mapping
        firstFlight.DayIndex.Should().Be(dayIndex);
        secondFlight.DayIndex.Should().Be(dayIndex);

        // First day mapping
        firstFlight.FlightNumber.Should().Be(firstInputFlight.FlightNumber);
        firstFlight.OriginCity.Should().Be(firstInputFlight.OriginCity);
        firstFlight.OriginAirportCode.Should().Be(firstInputFlight.OriginAirportCode);
        firstFlight.DestinationCity.Should().Be(firstInputFlight.DestinationCity);
        firstFlight.DestinationAirportCode.Should().Be(firstInputFlight.DestinationAirportCode);

        // Second day mapping
        secondFlight.FlightNumber.Should().Be(secondInputFlight.FlightNumber);
        secondFlight.OriginCity.Should().Be(secondInputFlight.OriginCity);
        secondFlight.OriginAirportCode.Should().Be(secondInputFlight.OriginAirportCode);
        secondFlight.DestinationCity.Should().Be(secondInputFlight.DestinationCity);
        secondFlight.DestinationAirportCode.Should().Be(secondInputFlight.DestinationAirportCode);

        flightRepositoryMock.Verify(x => x.AddFlights(It.Is<List<Domain.Flight>>(x =>
            x.Select(x => x.FlightNumber).Contains(firstInputFlight.FlightNumber) &&
            x.Select(x => x.FlightNumber).Contains(secondInputFlight.FlightNumber))), Times.Once);
        
        flightRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task Handler_PassEmptyFlights_ThrowsException()
    {
        //Setup
        var mediatorMock = new Mock<IMediator>();
        var flightRepositoryMock = new Mock<IFlightRepository>();
        var ordersRepositoryMock = new Mock<IOrdersRepository>();
        
        var handler = new LoadFlightScheduleCommandHandler(flightRepositoryMock.Object, ordersRepositoryMock.Object,
            mediatorMock.Object);

        //Act
       
        //Assert
        var exception = await Assert.ThrowsAsync<ApplicationLogicException>(() => handler.Handle(new LoadFlightScheduleCommand(), default));
        exception.Message.Should().Contain("Schedule is empty");
    }
}