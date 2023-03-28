using Microsoft.EntityFrameworkCore;
using SpeedyAir.Domain;

namespace SpeedyAir.Infrastructure;

public class SpeedyAirDbContext : DbContext
{
    public SpeedyAirDbContext(
        DbContextOptions<SpeedyAirDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Flight> Flights
    {
        get; set;
    }
    
    public DbSet<Order> Orders
    {
        get; set;
    }
}