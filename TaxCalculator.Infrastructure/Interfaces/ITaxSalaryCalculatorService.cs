
using TaxCalculator.Data.Models;
using TaxCalculator.Entities.Models;

namespace TaxCalculator.Infrastructure.Interfaces;

public interface ITaxSalaryCalculatorService
{
    Task<TaxSalary?> CalculateIncomeTaxAsync(decimal grossAnnualSalary);
    
    Task<List<TaxCalculatorHistory>?> GetHistoryAsync();
}