using SpeedyAir.Domain;
using SpeedyAir.Infrastructure.Aggregates.Flight;
using SpeedyAir.Infrastructure.Aggregates.Order;

namespace SpeedyAir.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependenciesBootstrapper
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SpeedyAirDbContext>(options =>
            {

                options.UseSqlServer(
                        configuration.GetConnectionString("BNineConnection"),
                        b => b.MigrationsAssembly(typeof(SpeedyAirDbContext).Assembly.FullName));

                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<IFlightRepository, FlightRepository>();
            services.AddScoped<IOrdersRepository, OrdersRepository>();

            return services;
        }
    }
}
