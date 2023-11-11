namespace TaxCalculator.Data.Models;

public class TaxCalculatorHistory
{
    public long Id { get; set; }
    public decimal GrossAnnualSalary { get; set; }
    public decimal GrossMonthlySalary { get; set; }
    public decimal NetAnnualSalary { get; set; }
    public decimal NetMonthlySalary { get; set; }
    public decimal AnnualTaxPaid { get; set; }
    public decimal MonthlyTaxPaid { get; set; }
    public DateTime CreatedAt { get; set; }
}