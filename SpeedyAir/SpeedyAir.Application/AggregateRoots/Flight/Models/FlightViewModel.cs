namespace SpeedyAir.Application.AggregateRoots.Flight.Models;

public class FlightViewModel
{
    public int FlightNumber { get; set; }
    
    public string OriginCity { get; set; }

    public string OriginAirportCode { get; set; }

    public string DestinationCity { get; set; }
    
    public string DestinationAirportCode { get; set; }
}