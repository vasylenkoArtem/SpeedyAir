namespace SpeedyAir.Infrastructure;

public interface IRepositoryBase
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}