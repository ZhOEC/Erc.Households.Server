using Microsoft.EntityFrameworkCore.Migrations;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class AddPeriodIdTaxInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "period_id",
                table: "tax_invoices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_tax_invoices_period_id_branch_office_id",
                table: "tax_invoices",
                columns: new[] { "period_id", "branch_office_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tax_invoices_period_id_branch_office_id",
                table: "tax_invoices");

            migrationBuilder.DropColumn(
                name: "period_id",
                table: "tax_invoices");
        }
    }
}
