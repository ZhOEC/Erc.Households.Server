using Microsoft.EntityFrameworkCore.Migrations;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class updatetaxinvoiceindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tax_invoices_period_id_branch_office_id",
                table: "tax_invoices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_tax_invoices_period_id_branch_office_id",
                table: "tax_invoices",
                columns: new[] { "period_id", "branch_office_id" },
                unique: true);
        }
    }
}
