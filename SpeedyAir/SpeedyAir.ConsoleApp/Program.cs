using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using SpeedyAir.Application;
using SpeedyAir.Application.Exceptions;
using SpeedyAir.ConsoleApp.Exceptions;
using SpeedyAir.ConsoleApp.Services;
using SpeedyAir.Domain.Exceptions;
using SpeedyAir.Infrastructure;

namespace SpeedyAir.ConsoleApp;

internal class Program
{
    private static readonly Logger StaticLogger = LogManager.GetCurrentClassLogger();

    private static async Task RunTakeHomeScenario(IServiceScope serviceScope)
    {
        var flightScheduleService = serviceScope.ServiceProvider.GetRequiredService<IFlightScheduleConsoleService>();
        var ordersService = serviceScope.ServiceProvider.GetRequiredService<IOrdersLoadingConsoleService>();
        
        await flightScheduleService.LoadFlightSchedule();
        
        await ordersService.LoadOrders();
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
        catch (DomainException ex)
        {
            Console.WriteLine($"{nameof(DomainException)} exception happened, message: {ex.Message}");
        }
        catch (ApplicationLogicException ex)
        {
            Console.WriteLine($"{nameof(ApplicationLogicException)} exception happened, message: {ex.Message}");
        }
        catch (ConsoleAppLogicException ex)
        {
            Console.WriteLine($"{nameof(ConsoleAppLogicException)} exception happened, message: {ex.Message}");
        }
        catch (Exception ex)
        {
            StaticLogger.Error("Error has been occured,{ex}", ex);
            Console.WriteLine("Unhandled exception happened");
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
