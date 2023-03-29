using MediatR;
using SpeedyAir.Application.AggregateRoots.Order.Models;

namespace SpeedyAir.Application.AggregateRoots.Order.Commands;

public class AddOrdersCommand : IRequest<List<int>>
{
    public List<AddOrderViewModel> Orders { get; set; }
}
