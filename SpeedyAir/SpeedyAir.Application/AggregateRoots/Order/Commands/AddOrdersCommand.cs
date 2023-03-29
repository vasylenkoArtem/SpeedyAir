using MediatR;
using SpeedyAir.Application.AggregateRoots.Order.Models;

namespace SpeedyAir.Application.AggregateRoots.Order.Commands;

// Save to db
public class AddOrdersCommand : IRequest<List<int>>
{
    public List<AddOrderViewModel> Orders { get; set; }
}
