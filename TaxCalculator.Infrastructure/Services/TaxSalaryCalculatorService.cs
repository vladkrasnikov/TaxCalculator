using Microsoft.EntityFrameworkCore;
using TaxCalculator.Data.Interfaces;
using TaxCalculator.Data.Models;
using TaxCalculator.Entities.Constants;
using TaxCalculator.Entities.Models;
using TaxCalculator.Infrastructure.Interfaces;

namespace TaxCalculator.Infrastructure.Services;

public class TaxSalaryCalculatorService : ITaxSalaryCalculatorService
{
    private readonly ITaxCalculatorContext _dbContext;
    
    public TaxSalaryCalculatorService(ITaxCalculatorContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task<TaxSalary?> CalculateIncomeTaxAsync(decimal grossAnnualSalary)
    {
        if (grossAnnualSalary < 0)
        {
            throw new ArgumentException("Gross annual salary cannot be less than 0");
        }

        var taxCaclRepo = _dbContext.GetRepository<TaxCalculatorHistory>();

        if (taxCaclRepo == null)
        {
            throw new NullReferenceException("Tax calculator repository is null");
        }
        
        var taxSalary = CalculateTaxes(grossAnnualSalary);

        taxCaclRepo.Add(new TaxCalculatorHistory
        {
            GrossAnnualSalary = taxSalary.GrossAnnualSalary,
            GrossMonthlySalary = taxSalary.GrossMonthlySalary,
            NetAnnualSalary = taxSalary.NetAnnualSalary,
            NetMonthlySalary = taxSalary.NetMonthlySalary,
            AnnualTaxPaid = taxSalary.AnnualTaxPaid,
            MonthlyTaxPaid = taxSalary.MonthlyTaxPaid,
            CreatedAt = DateTime.UtcNow
        });

        await taxCaclRepo.SaveChangesAsync();

        return await Task.FromResult(taxSalary);
    }

    public async Task<List<TaxCalculatorHistory>?> GetHistoryAsync()
    {
        return await _dbContext.GetRepository<TaxCalculatorHistory>().GetAll().ToListAsync();
    }

    private TaxSalary CalculateTaxes(decimal grossAnnualSalary)
    {
        decimal totalTax = 0;

        foreach (var band in ApiConstants.TaxBands)
        {
            decimal taxableAmount = Math.Min(grossAnnualSalary, band.UpperLimit) - band.LowerLimit;

            if (taxableAmount > 0)
            {
                decimal taxInBand = (taxableAmount * band.TaxRate) / 100;
                totalTax += taxInBand;
            }

            if (grossAnnualSalary <= band.UpperLimit)
                break;
        }

        return new TaxSalary()
        {
            AnnualTaxPaid = totalTax,
            NetAnnualSalary = grossAnnualSalary - totalTax,
            GrossAnnualSalary = grossAnnualSalary,
            GrossMonthlySalary = grossAnnualSalary / 12,
            NetMonthlySalary = (grossAnnualSalary - totalTax) / 12,
            MonthlyTaxPaid = totalTax / 12
        };
    }
}