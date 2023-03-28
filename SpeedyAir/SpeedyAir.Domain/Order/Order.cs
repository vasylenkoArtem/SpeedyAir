namespace SpeedyAir.Domain.Order;

public class Order
{
    public int Id { get; set; }

    public string OrderIdentifier { get; set; }

    public Flight Flight { get; set; }
}