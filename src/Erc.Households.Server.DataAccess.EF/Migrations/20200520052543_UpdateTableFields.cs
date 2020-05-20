using Microsoft.EntityFrameworkCore.Migrations;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class UpdateTableFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "channel_id",
                table: "payment_batches");

            migrationBuilder.DropColumn(
                name: "total_amount",
                table: "payment_batches");

            migrationBuilder.DropColumn(
                name: "total_count",
                table: "payment_batches");

            migrationBuilder.DropColumn(
                name: "total_amount_sales",
                table: "invoices");

            migrationBuilder.AddColumn<int>(
                name: "branch_office_id",
                table: "payment_batches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "payment_channel_id",
                table: "payment_batches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "total_sales",
                table: "invoices",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "ix_invoice_accounting_point_id",
                table: "invoices",
                column: "accounting_point_id");

            migrationBuilder.AddForeignKey(
                name: "fk_invoice_accounting_points_accounting_point_id",
                table: "invoices",
                column: "accounting_point_id",
                principalTable: "accounting_points",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_invoice_accounting_points_accounting_point_id",
                table: "invoices");

            migrationBuilder.DropIndex(
                name: "ix_invoice_accounting_point_id",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "branch_office_id",
                table: "payment_batches");

            migrationBuilder.DropColumn(
                name: "payment_channel_id",
                table: "payment_batches");

            migrationBuilder.DropColumn(
                name: "total_sales",
                table: "invoices");

            migrationBuilder.AddColumn<int>(
                name: "channel_id",
                table: "payment_batches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "total_amount",
                table: "payment_batches",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "total_count",
                table: "payment_batches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "total_amount_sales",
                table: "invoices",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
