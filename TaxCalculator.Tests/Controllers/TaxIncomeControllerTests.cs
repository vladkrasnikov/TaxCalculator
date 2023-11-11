using Moq;
using TaxCalculator.API.Controllers;
using TaxCalculator.Data.Models;
using TaxCalculator.Entities.Models;
using TaxCalculator.Infrastructure.Interfaces;
using Xunit;

namespace TaxCalculator.Tests.Controllers;

public class TaxIncomeControllerTests
{
    private readonly Mock<ITaxSalaryCalculatorService> _salaryCalculatorServiceMock;
    private readonly TaxIncomeController _taxIncomeController;
    
    public TaxIncomeControllerTests()
    {
        _salaryCalculatorServiceMock = new Mock<ITaxSalaryCalculatorService>();
        _taxIncomeController = new TaxIncomeController(_salaryCalculatorServiceMock.Object);
    }

    [Fact]
    public async Task GetTaxIncomeAsync_ReceiveGrossAnnualSalary_ReturnTaxIncomeWithProperGrossAnnualSalary()
    {
        // Arrange
        var grossAnnualSalary = 100000;
        var annualTaxPaid = 1000;
        var taxIncome = new TaxSalary()
        {
            GrossAnnualSalary = grossAnnualSalary,
            GrossMonthlySalary = grossAnnualSalary / 12,
            NetAnnualSalary = grossAnnualSalary - annualTaxPaid,
            NetMonthlySalary = (grossAnnualSalary - annualTaxPaid) / 12,
            AnnualTaxPaid = annualTaxPaid,
            MonthlyTaxPaid = annualTaxPaid / 12
        };
        _salaryCalculatorServiceMock.Setup(x => x.CalculateIncomeTaxAsync(grossAnnualSalary)).ReturnsAsync(taxIncome);
        var controller = (_salaryCalculatorServiceMock.Object);
        
        // Act
        var result = await controller.CalculateIncomeTaxAsync(grossAnnualSalary);
        
        // Assert
        Assert.Equal(taxIncome, result);
    }
    
    [Fact]
    public async Task GetTaxIncomeAsync_ReceiveGrossAnnualSalary_ReturnTaxIncomeWithProperGrossMonthlySalary()
    {
        // Arrange
        var taxHistoryList = new List<TaxCalculatorHistory>()
        {
            new()
            {
                GrossAnnualSalary = 100000,
                GrossMonthlySalary = 100000 / 12,
                NetAnnualSalary = 100000 - 1000,
                NetMonthlySalary = (100000 - 1000) / 12,
                AnnualTaxPaid = 1000,
                MonthlyTaxPaid = 1000 / 12,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                GrossAnnualSalary = 40000,
                GrossMonthlySalary = 40000 / 12,
                NetAnnualSalary = 40000 - 1000,
                NetMonthlySalary = (40000 - 11000) / 12,
                AnnualTaxPaid = 11000,
                MonthlyTaxPaid = 11000 / 12,
                CreatedAt = DateTime.UtcNow
            }
        };
        _salaryCalculatorServiceMock.Setup(x => x.GetHistoryAsync()).ReturnsAsync(taxHistoryList);
        var controller = (_salaryCalculatorServiceMock.Object);
        
        // Act
        var result = await controller.GetHistoryAsync();
        
        // Assert
        Assert.Equal(taxHistoryList, result);
    }
}