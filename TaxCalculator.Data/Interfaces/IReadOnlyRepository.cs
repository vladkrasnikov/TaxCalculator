namespace TaxCalculator.Data.Interfaces;

public interface IReadOnlyRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();
}