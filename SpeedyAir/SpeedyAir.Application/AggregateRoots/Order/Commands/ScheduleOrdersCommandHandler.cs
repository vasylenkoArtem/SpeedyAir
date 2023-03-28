using MediatR;
using SpeedyAir.Application.AggregateRoots.Order.Models;

namespace SpeedyAir.Application.AggregateRoots.Order.Commands;

public class ScheduleOrdersCommandHandler: IRequestHandler<ScheduleOrdersCommand, List<OrderViewModel>>
{
    public async Task<List<OrderViewModel>> Handle(ScheduleOrdersCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}