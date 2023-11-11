using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxCalculator.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class InitialTaxCalculatorHistoryDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrossAnnualSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossMonthlySalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAnnualSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetMonthlySalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AnnualTaxPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyTaxPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxHistory", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxHistory");
        }
    }
}
