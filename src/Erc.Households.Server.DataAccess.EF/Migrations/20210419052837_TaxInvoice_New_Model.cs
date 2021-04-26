using System.Collections.Generic;
using Erc.Households.Domain.Taxes;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class TaxInvoice_New_Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tariff_value",
                table: "tax_invoices");

            migrationBuilder.AddColumn<IEnumerable<TaxInvoiceTabLine>>(
                name: "tab_lines",
                table: "tax_invoices",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tab_lines",
                table: "tax_invoices");

            migrationBuilder.AddColumn<decimal>(
                name: "tariff_value",
                table: "tax_invoices",
                type: "decimal(9,8)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
