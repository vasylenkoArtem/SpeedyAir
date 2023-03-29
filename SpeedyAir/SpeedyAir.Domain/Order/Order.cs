using System.ComponentModel.DataAnnotations;

namespace SpeedyAir.Domain;

public class Order
{
    public Order()
    {
        
    }

    public Order(string orderIdentifier, string destinationAirportCode, string originAirportCode)
    {
        OrderIdentifier = orderIdentifier;
        OriginAirportCode = originAirportCode;
        DestinationAirportCode = destinationAirportCode;
    }

    [Key]
    public int Id { get; set; }

    public string OrderIdentifier { get; set; }

    public string OriginAirportCode { get; set; }

    public string DestinationAirportCode { get; set; }
    
    public int? FlightId { get; set; }

    public Flight Flight { get; set; }
}