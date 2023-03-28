using Microsoft.EntityFrameworkCore;
using SpeedyAir.Domain;
using SpeedyAir.Domain.Exceptions;

namespace SpeedyAir.Infrastructure.Aggregates.Flight;

public class FlightRepository : RepositoryBase, IFlightRepository
{
    public FlightRepository(SpeedyAirDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task AddFlights(List<Domain.Flight> flights)
    {
        var flightNumbers = flights.Select(x => x.FlightNumber);

        var existingFlight = await _dbContext.Flights.FirstOrDefaultAsync(
            x => flightNumbers.Contains(x.FlightNumber));

        if (existingFlight != null)
        {
            throw new DomainException($"Flight with number {existingFlight.FlightNumber} already exists");
        }

        await _dbContext.Flights.AddRangeAsync(flights);
    }

    public async Task<List<Domain.Flight>> GetAvailableFlights()
    {
        var flights = await _dbContext.Flights
            .Where(x => x.Orders.Count < x.MaxAmountOfBoxes)
            .ToListAsync();

        return flights;
    }
}