using MediatR;

namespace SpeedyAir.Application.AggregateRoots.Order.Commands;

public class AddOrdersCommandHandler : IRequestHandler<AddOrdersCommand, List<int>>
{
    public async Task<List<int>> Handle(AddOrdersCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}