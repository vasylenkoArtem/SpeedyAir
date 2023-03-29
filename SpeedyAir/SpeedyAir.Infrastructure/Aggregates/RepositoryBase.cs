namespace SpeedyAir.Infrastructure;

public abstract class RepositoryBase : IRepositoryBase, IDisposable
{
    protected SpeedyAirDbContext _dbContext;

    protected RepositoryBase(SpeedyAirDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    
    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}