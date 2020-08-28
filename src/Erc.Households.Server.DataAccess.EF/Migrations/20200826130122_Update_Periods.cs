using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class Update_Periods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_branch_offices_period_current_period_id",
                table: "branch_offices");

            migrationBuilder.DropForeignKey(
                name: "fk_invoices_period_period_id",
                table: "invoices");

            migrationBuilder.DropForeignKey(
                name: "fk_payments_period_period_id",
                table: "payments");

            migrationBuilder.DropPrimaryKey(
                name: "pk_period",
                table: "periods");

            migrationBuilder.AlterColumn<decimal>(
                name: "debt",
                table: "accounting_points",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddPrimaryKey(
                name: "pk_periods",
                table: "periods",
                column: "id");

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202001,
                column: "end_date",
                value: new DateTime(2020, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202002,
                column: "end_date",
                value: new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202003,
                column: "end_date",
                value: new DateTime(2020, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202004,
                column: "end_date",
                value: new DateTime(2020, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202005,
                column: "end_date",
                value: new DateTime(2020, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202006,
                column: "end_date",
                value: new DateTime(2020, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202007,
                column: "end_date",
                value: new DateTime(2020, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "fk_branch_offices_periods_current_period_id",
                table: "branch_offices",
                column: "current_period_id",
                principalTable: "periods",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_invoices_periods_period_id",
                table: "invoices",
                column: "period_id",
                principalTable: "periods",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_payments_periods_period_id",
                table: "payments",
                column: "period_id",
                principalTable: "periods",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_branch_offices_periods_current_period_id",
                table: "branch_offices");

            migrationBuilder.DropForeignKey(
                name: "fk_invoices_periods_period_id",
                table: "invoices");

            migrationBuilder.DropForeignKey(
                name: "fk_payments_periods_period_id",
                table: "payments");

            migrationBuilder.DropPrimaryKey(
                name: "pk_periods",
                table: "periods");

            migrationBuilder.AlterColumn<decimal>(
                name: "debt",
                table: "accounting_points",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddPrimaryKey(
                name: "pk_period",
                table: "periods",
                column: "id");

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202001,
                column: "end_date",
                value: new DateTime(2019, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202002,
                column: "end_date",
                value: new DateTime(2019, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202003,
                column: "end_date",
                value: new DateTime(2019, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202004,
                column: "end_date",
                value: new DateTime(2019, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202005,
                column: "end_date",
                value: new DateTime(2019, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202006,
                column: "end_date",
                value: new DateTime(2019, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202007,
                column: "end_date",
                value: new DateTime(2019, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "fk_branch_offices_period_current_period_id",
                table: "branch_offices",
                column: "current_period_id",
                principalTable: "periods",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_invoices_period_period_id",
                table: "invoices",
                column: "period_id",
                principalTable: "periods",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_payments_period_period_id",
                table: "payments",
                column: "period_id",
                principalTable: "periods",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
