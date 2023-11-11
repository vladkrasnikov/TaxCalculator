using Microsoft.EntityFrameworkCore;
using TaxCalculator.API.Middleware;
using TaxCalculator.Data.Contexts;
using TaxCalculator.Data.Interfaces;
using TaxCalculator.Infrastructure.Interfaces;
using TaxCalculator.Infrastructure.Services;

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ITaxCalculatorContext, TaxCalculatorContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaxCalculatorDatabase")));

builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins,
        policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

builder.Services.AddScoped<ITaxSalaryCalculatorService, TaxSalaryCalculatorService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
