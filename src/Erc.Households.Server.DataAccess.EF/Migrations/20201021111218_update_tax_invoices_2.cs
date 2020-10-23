using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class update_tax_invoices_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lliability_date",
                table: "tax_invoices");

            migrationBuilder.RenameColumn(
                name: "lliability_sum",
                table: "tax_invoices",
                newName: "liability_sum");

            migrationBuilder.AddColumn<DateTime>(
                name: "liability_date",
                table: "tax_invoices",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "liability_date",
                table: "tax_invoices");

            migrationBuilder.RenameColumn(
                name: "liability_sum",
                table: "tax_invoices",
                newName: "lliability_sum");

            migrationBuilder.AddColumn<DateTime>(
                name: "lliability_date",
                table: "tax_invoices",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
