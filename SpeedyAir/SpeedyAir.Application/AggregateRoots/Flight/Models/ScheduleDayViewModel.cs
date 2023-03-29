namespace SpeedyAir.Application.AggregateRoots.Flight.Models;

public class ScheduleDayViewModel
{
    public List<CreateFlightViewModel> Flights { get; set; }
 
    public int DayIndex { get; set; }
}
