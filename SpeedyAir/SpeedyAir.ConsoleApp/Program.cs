using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using SpeedyAir.Application;
using SpeedyAir.ConsoleApp.Services;
using SpeedyAir.Infrastructure;

namespace SpeedyAir.ConsoleApp;

internal class Program
{
    private static readonly Logger StaticLogger = LogManager.GetCurrentClassLogger();

    private static async Task RunTakeHomeScenario(IServiceScope serviceScope)
    {
        var flightScheduleService = serviceScope.ServiceProvider.GetRequiredService<IFlightScheduleService>();
        var ordersService = serviceScope.ServiceProvider.GetRequiredService<IOrdersService>();
        
        await flightScheduleService.LoadFlightSchedule();
        
        await ordersService.LoadOrders();
        
        //TODO: process not scheduled orders Integration Event, trigger when new flight added
    }

    internal static async Task Main(string[] args)
    {
        try
        {
            StaticLogger.Info("Starting Console Application");

            var configuration = BuildConfiguration();

            var provider = ConfigureServices(configuration);

            var scope = provider.CreateScope();

            StaticLogger.Info("Configured successfully");

            await RunTakeHomeScenario(scope);
        }
        catch (Exception ex)
        {
            StaticLogger.Error("Error has been occured,{ex}", ex);
            throw;
        }
    }

    private static IServiceProvider ConfigureServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();

        services.AddTransient<IConfiguration>(x => configuration);

        services.AddSqlServerDatabase(configuration);
        services.AddConsoleAppServices();
        services.AddApplication();
        
        return services.BuildServiceProvider(true);
    }

    private static IConfiguration BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        return builder.Build();
    }

}
