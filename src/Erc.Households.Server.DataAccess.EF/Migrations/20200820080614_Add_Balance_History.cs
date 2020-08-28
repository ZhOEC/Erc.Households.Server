using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class Add_Balance_History : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounting_point_debt_history",
                columns: table => new
                {
                    accounting_point_id = table.Column<int>(nullable: false),
                    period_id = table.Column<int>(nullable: false),
                    debt_value = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounting_point_debt_history", x => new { x.accounting_point_id, x.period_id });
                });

            //migrationBuilder.InsertData(
            //    table: "periods",
            //    columns: new[] { "id", "end_date", "name", "start_date" },
            //    values: new object[] { 202007, new DateTime(2020, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Липень 2020р.", new DateTime(2020, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounting_point_debt_history");

            migrationBuilder.DeleteData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202007);
        }
    }
}
