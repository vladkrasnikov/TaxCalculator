using Microsoft.EntityFrameworkCore;
using TaxCalculator.Data.Interfaces;

namespace TaxCalculator.Data.Abstractions;

public class CrudRepository<TEntity> : ReadOnlyRepository<TEntity>, IRepository<TEntity> where TEntity : class
{
    public CrudRepository(DbContext dbContext)
        : base(dbContext)
    {
    }

    public void Add(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public void Delete(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public override IQueryable<TEntity> GetAll()
    {
        return DbSet;
    }

    public async Task SaveChangesAsync()
    {
        await Context.SaveChangesAsync();
    }

    public void Update(TEntity entity)
    {
        Context.Update(entity);
    }
}