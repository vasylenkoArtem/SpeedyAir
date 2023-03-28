namespace SpeedyAir.Application.AggregateRoots.Order.Models;

public class AddOrderViewModel
{
    public string OriginAirportCode { get; set; }
    
    public string DestinationAirportCode { get; set; }

    public string OrderIdentificator { get; set; }
}