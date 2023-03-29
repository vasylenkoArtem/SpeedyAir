using System.ComponentModel.DataAnnotations;

namespace SpeedyAir.Domain;

public class Flight
{
    public Flight()
    { }
    
    public Flight(int flightNumber, 
        string originCity, string originAirportCode, 
        string destinationCity,
        string destinationAirportCode, 
        int departureDayIndex, 
        //SpeedyAir.ly by default
        string airlineDesignator = "SA",
        int maxAmountOfBoxes = 20)
    {
        OriginCity = originCity;
        OriginAirportCode = originAirportCode;
        DestinationCity = destinationCity;
        DestinationAirportCode = destinationAirportCode;
        AirlineDesignator = airlineDesignator;
        MaxAmountOfBoxes = maxAmountOfBoxes;
        DepartureDay = departureDayIndex;
        FlightNumber = flightNumber;
    }
    
    [Key]
    public int Id { get; set; }

    public int FlightNumber { get; set; }
    
    public string AirlineDesignator { get; set; }
    
    public string OriginCity { get; set; }

    public string OriginAirportCode { get; set; }

    public string DestinationCity { get; set; }
    
    public string DestinationAirportCode { get; set; }
    
    public int DepartureDay { get; set; }
    
    public int MaxAmountOfBoxes { get; set; }

    
    public List<Order> Orders { get; set; }
}