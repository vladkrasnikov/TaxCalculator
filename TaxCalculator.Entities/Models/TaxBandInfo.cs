namespace TaxCalculator.Entities.Models;

public class TaxBandInfo
{
    public decimal LowerLimit { get; }
    public decimal UpperLimit { get; }
    public decimal TaxRate { get; }
    
    public TaxBandInfo(decimal lowerLimit, decimal upperLimit, decimal taxRate)
    {
        LowerLimit = lowerLimit;
        UpperLimit = upperLimit;
        TaxRate = taxRate;
    }
}