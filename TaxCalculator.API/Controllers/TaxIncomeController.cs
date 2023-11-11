using Microsoft.AspNetCore.Mvc;
using TaxCalculator.Data.Models;
using TaxCalculator.Entities.Models;
using TaxCalculator.Infrastructure.Interfaces;

namespace TaxCalculator.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TaxIncomeController : ControllerBase
{
    private readonly ITaxSalaryCalculatorService _taxSalaryCalculatorService;

    public TaxIncomeController(ITaxSalaryCalculatorService taxSalaryCalculatorService)
    {
        _taxSalaryCalculatorService = taxSalaryCalculatorService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TaxSalary), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<TaxSalary?> CalculateIncomeTax([FromBody] int grossAnnualSalary)
    {
        return await _taxSalaryCalculatorService.CalculateIncomeTaxAsync(grossAnnualSalary);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<TaxCalculatorHistory>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<List<TaxCalculatorHistory>?> GetHistory()
    {
        return await _taxSalaryCalculatorService.GetHistoryAsync();
    }
}