namespace SpeedyAir.Application.AggregateRoots.Order.Models;

public class OrderViewModel
{
    public string OrderIdentifier { get; set; }
    
    public int? FlightNumber { get; set; }
    
    public string DepartureCity { get; set; }

    public string ArrivalCity { get; set; }

    public int? DayIndex { get; set; }
}
