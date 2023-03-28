namespace SpeedyAir.Application.AggregateRoots.Flight.Models;

public class ScheduleDayViewModel
{
    public List<FlightViewModel> Flights { get; set; }
 
    public int DayIndex { get; set; }
}
