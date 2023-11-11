using Microsoft.EntityFrameworkCore;
using TaxCalculator.Data.Abstractions;
using TaxCalculator.Data.Interfaces;
using TaxCalculator.Data.Models;

namespace TaxCalculator.Data.Contexts;

public class TaxCalculatorContext : DbContext, ITaxCalculatorContext
{
    public TaxCalculatorContext(DbContextOptions<TaxCalculatorContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<TaxCalculatorHistory> TaxHistory { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaxCalculatorHistory>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.GrossAnnualSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GrossMonthlySalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NetAnnualSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NetMonthlySalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AnnualTaxPaid).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MonthlyTaxPaid).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
        });
    }
    
    #region Interface Methods

    public IRepository<TEntity> GetRepository<TEntity>()
        where TEntity : class
    {
        return new CrudRepository<TEntity>(this);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
    
    #endregion
}