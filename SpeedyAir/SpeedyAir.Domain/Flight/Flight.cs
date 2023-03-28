namespace SpeedyAir.Domain;

public class Flight
{
    public int Id { get; set; }

    public int FlightNumber { get; set; }
    
    public string AirlineDesignator { get; set; }
    
    public string OriginCity { get; set; }

    public string OriginAirportCode { get; set; }

    public string DestinationCity { get; set; }
    
    public string DestinationAirportCode { get; set; }
    
    public int DepartureDay { get; set; }
    
    public int MaxAmountOfBoxes { get; set; }
}