using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SpeedyAir.Domain;

namespace SpeedyAir.Infrastructure;

public class SpeedyAirDbContext : DbContext
{
    public SpeedyAirDbContext()
    {
    }

    public SpeedyAirDbContext([NotNull] DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder();
    
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        IConfiguration Configuration = builder.Build();
    
        optionsBuilder.UseSqlServer(
            Configuration.GetConnectionString("DatabaseConnection"));
        base.OnConfiguring(optionsBuilder);
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