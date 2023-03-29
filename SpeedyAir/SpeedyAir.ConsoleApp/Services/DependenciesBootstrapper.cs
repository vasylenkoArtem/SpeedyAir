namespace SpeedyAir.ConsoleApp.Services
{
    using Microsoft.Extensions.DependencyInjection;

    public static class DependenciesBootstrapper
    {
        public static IServiceCollection AddConsoleAppServices(this IServiceCollection services)
        {
            services.AddTransient<IFlightScheduleService, FlightScheduleService>();
            services.AddTransient<IOrdersService, OrdersService>();
            
            return services;
        }
    }
}
