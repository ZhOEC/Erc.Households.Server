using Microsoft.EntityFrameworkCore.Migrations;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class Update_AccountingPoint_Name_UniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_accounting_points_name",
                table: "accounting_points");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_name_branch_office_id",
                table: "accounting_points",
                columns: new[] { "name", "branch_office_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_accounting_points_name_branch_office_id",
                table: "accounting_points");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_name",
                table: "accounting_points",
                column: "name",
                unique: true);
        }
    }
}
