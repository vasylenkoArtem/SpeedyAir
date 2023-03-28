using SpeedyAir.Infrastructure;

namespace SpeedyAir.Domain;

public interface IFlightRepository : IRepositoryBase
{
    Task AddFlights(List<Domain.Flight> flights);
    
    Task<List<Flight>> GetAvailableFlights();
}
