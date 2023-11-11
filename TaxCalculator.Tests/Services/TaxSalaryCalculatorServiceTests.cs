using FluentAssertions;
using Moq;
using MockQueryable.Moq;
using TaxCalculator.Data.Interfaces;
using TaxCalculator.Data.Models;
using TaxCalculator.Entities.Models;
using TaxCalculator.Infrastructure.Services;
using Xunit;

namespace TaxCalculator.Tests.Services;

public class TaxSalaryCalculatorServiceTests
{
    private readonly Mock<IRepository<TaxCalculatorHistory>> _taxCalculatorHistoryRepositoryMock = new();
    private readonly Mock<ITaxCalculatorContext> _taxCalculatorContextMock = new();
    
    public TaxSalaryCalculatorServiceTests()
    {
        _taxSalaryCalculatorService = new TaxSalaryCalculatorService(_taxCalculatorContextMock.Object);
    }

    private TaxSalaryCalculatorService _taxSalaryCalculatorService;

    #region Theory Data

    public static IList<object[]> CalculateTaxData =>
        new List<object[]>
        {
            new object[]
            {
                10000,
                new TaxSalary
                {
                    GrossAnnualSalary = 10000,
                    GrossMonthlySalary = (decimal)10000 / 12,
                    NetAnnualSalary = 10000 - 1000,
                    NetMonthlySalary = (decimal)(10000 - 1000) / 12,
                    AnnualTaxPaid = 1000,
                    MonthlyTaxPaid = (decimal)1000 / 12
                },
                Times.Once(),
                null
            },
            new object[]
            {
                40000,
                new TaxSalary
                {
                    GrossAnnualSalary = 40000,
                    GrossMonthlySalary = (decimal)40000 / 12,
                    NetAnnualSalary = 40000 - 11000,
                    NetMonthlySalary = (decimal)(40000 - 11000) / 12,
                    AnnualTaxPaid = 11000,
                    MonthlyTaxPaid = (decimal)11000 / 12
                },
                Times.Once(),
                null
            },
            new object[]
            {
                -1,
                null,
                Times.Never(),
                typeof(ArgumentException)
            }
        };
    
    public static IList<object[]> TaxHistoryData =>
        new List<object[]>
        {
            new object[]
            {
                new List<TaxCalculatorHistory>()
                {
                    new()
                    {
                        GrossAnnualSalary = 10000,
                        GrossMonthlySalary = 10000 / 12,
                        NetAnnualSalary = 10000 - 1000,
                        NetMonthlySalary = (10000 - 1000) / 12,
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
                },
                Times.Once(),
                null
            }
        };

    #endregion

    [Theory]
    [MemberData(nameof(CalculateTaxData))]
    public async Task CalculateTaxesAsync_ReceiveGrossAnnualSalary_ReturnTaxIncomeWithProperGrossAnnualSalary(
        decimal grossAnnualSalary, TaxSalary expectedTaxSalary, Times times, Type? expectedExceptionType)
    {
        // Arrange
        _taxCalculatorContextMock.Setup(x => x.GetRepository<TaxCalculatorHistory>())
            .Returns(_taxCalculatorHistoryRepositoryMock.Object);
        _taxCalculatorHistoryRepositoryMock.Setup(x => x.Add(It.IsAny<TaxCalculatorHistory>()));
        _taxCalculatorHistoryRepositoryMock.Setup(x => x.SaveChangesAsync());

        // Act
        TaxSalary? result = null;
        Exception? ex = null;

        if (expectedExceptionType == null)
        {
            result = await _taxSalaryCalculatorService.CalculateIncomeTaxAsync(grossAnnualSalary);
        }
        else
        {
            ex = await Record.ExceptionAsync(async () => await _taxSalaryCalculatorService.CalculateIncomeTaxAsync(grossAnnualSalary));
        }

        // Assert
        _taxCalculatorHistoryRepositoryMock.Verify(x => x.Add(It.IsAny<TaxCalculatorHistory>()), times);
        _taxCalculatorHistoryRepositoryMock.Verify(x => x.SaveChangesAsync(), times);

        if (expectedExceptionType != null)
        {
            ex.Should().BeOfType(expectedExceptionType);
            result.Should().BeNull();
        }
        else
        {
            result.Should().BeEquivalentTo(expectedTaxSalary);
        }
    }
    
    [Theory]
    [MemberData(nameof(TaxHistoryData))]
    public async Task GetHistoryAsync_ReturnTaxCalculatorHistoryList(
        IEnumerable<TaxCalculatorHistory> taxHistoryList, Times times, Type? expectedExceptionType)
    {
        // Arrange
        IQueryable<TaxCalculatorHistory> taxHistoryListQueryable = taxHistoryList.BuildMock();
        
        _taxCalculatorContextMock.Setup(x => x.GetRepository<TaxCalculatorHistory>())
            .Returns(_taxCalculatorHistoryRepositoryMock.Object);
        _taxCalculatorHistoryRepositoryMock.Setup(x => x.GetAll())
            .Returns(taxHistoryListQueryable);

        // Act
        List<TaxCalculatorHistory>? result = null;
        Exception? ex = null;

        if (expectedExceptionType == null)
        {
            result = await _taxSalaryCalculatorService.GetHistoryAsync();
        }
        else
        {
            ex = await Record.ExceptionAsync(async () => await _taxSalaryCalculatorService.GetHistoryAsync());
        }

        // Assert
        _taxCalculatorHistoryRepositoryMock.Verify(x => x.GetAll(), times);

        if (expectedExceptionType != null)
        {
            ex.Should().BeOfType(expectedExceptionType);
            result.Should().BeNull();
        }
        else
        {
            result.Should().BeEquivalentTo(taxHistoryList);
        }
    }
}