using TaxCalculator.Entities.Models;

namespace TaxCalculator.Entities.Constants;

public static class ApiConstants
{
    public static readonly TaxBandInfo[] TaxBands = {
        new(0, 5000, 0),
        new(5000, 20000, 20),
        new(20000, decimal.MaxValue, 40)
    };
}