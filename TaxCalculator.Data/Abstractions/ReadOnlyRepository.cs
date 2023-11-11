using Microsoft.EntityFrameworkCore;
using TaxCalculator.Data.Interfaces;

namespace TaxCalculator.Data.Abstractions;

public abstract class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
{
    protected readonly DbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    protected DbContext Context
    {
        get { return DbContext; }
    }

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    protected ReadOnlyRepository(DbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TEntity>();
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        return DbSet.AsNoTracking();
    }
}