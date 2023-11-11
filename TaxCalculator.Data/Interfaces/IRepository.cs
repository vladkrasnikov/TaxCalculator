namespace TaxCalculator.Data.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();

    void Add(TEntity entity);

    void Delete(TEntity entity);

    Task SaveChangesAsync();

    void Update(TEntity entity);
}