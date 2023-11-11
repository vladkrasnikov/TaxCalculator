using Microsoft.EntityFrameworkCore;

namespace TaxCalculator.Data.Interfaces;

public interface ITaxCalculatorContext
{
    IRepository<TEntity> GetRepository<TEntity>()
        where TEntity : class;

    Task<int> SaveChangesAsync();

    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;
}