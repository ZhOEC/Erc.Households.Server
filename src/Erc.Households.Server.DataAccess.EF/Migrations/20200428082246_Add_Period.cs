using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Erc.Households.Server.DataAccess.EF.Migrations
{
    public partial class Add_Period : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "consumption_t1",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "consumption_t2",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "consumption_t3",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "paid_sum",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "present_meter_reading_t1",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "present_meter_reading_t2",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "present_meter_reading_t3",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "previous_meter_reading_t1",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "previous_meter_reading_t2",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "previous_meter_reading_t3",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "sales_t1",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "sales_t2",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "sales_t3",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "total_sum",
                table: "invoice");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "period",
                type: "citext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "total_record",
                table: "payment_channels",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "payment_channels",
                type: "citext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "accounting_point_id",
                table: "invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "period_id",
                table: "invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "present_t1meter_reading",
                table: "invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "present_t2meter_reading",
                table: "invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "present_t3meter_reading",
                table: "invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "previous_t1meter_reading",
                table: "invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "previous_t2meter_reading",
                table: "invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "previous_t3meter_reading",
                table: "invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "t1sales",
                table: "invoice",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "t1usage",
                table: "invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "t2sales",
                table: "invoice",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "t2usage",
                table: "invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "t3sales",
                table: "invoice",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "t3usage",
                table: "invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "total_amount_sales",
                table: "invoice",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "current_period_id",
                table: "branch_offices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "payment",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    batch_id = table.Column<int>(nullable: false),
                    accounting_point_id = table.Column<int>(nullable: true),
                    pay_date = table.Column<DateTime>(nullable: false),
                    enter_date = table.Column<DateTime>(nullable: false),
                    amount = table.Column<decimal>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    payer_info = table.Column<string>(nullable: true),
                    accounting_point_name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payment", x => x.id);
                    table.ForeignKey(
                        name: "fk_payment_accounting_points_accounting_point_id",
                        column: x => x.accounting_point_id,
                        principalTable: "accounting_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "invoice_payment_item",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    invoice_id = table.Column<int>(nullable: false),
                    payment_id = table.Column<int>(nullable: false),
                    amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_payment_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_payment_item_invoice_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoice",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invoice_payment_item_payment_payment_id",
                        column: x => x.payment_id,
                        principalTable: "payment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "period",
                columns: new[] { "id", "end_date", "name", "start_date" },
                values: new object[,]
                {
                    { 1, new DateTime(2019, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Січень 2019р.", new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, new DateTime(2019, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Квітень 2019р.", new DateTime(2020, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, new DateTime(2019, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Березень 2019р.", new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, new DateTime(2019, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Лютий 2019р.", new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, new DateTime(2019, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Січень 2019р.", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, new DateTime(2019, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Грудень 2019р.", new DateTime(2019, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, new DateTime(2019, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Листопад 2019р.", new DateTime(2019, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, new DateTime(2019, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Жовтень 2019р.", new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, new DateTime(2019, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Вересень 2019р.", new DateTime(2019, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, new DateTime(2019, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Серпень 2019р.", new DateTime(2019, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(2019, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Липень 2019р.", new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(2019, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Червень 2019р.", new DateTime(2019, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2019, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Травень 2019р.", new DateTime(2019, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2019, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Квітень 2019р.", new DateTime(2019, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2019, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Березень 2019р.", new DateTime(2019, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2019, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Лютий 2019р.", new DateTime(2019, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, new DateTime(2019, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Травень 2019р.", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, new DateTime(2019, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Червень 2019р.", new DateTime(2020, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 1,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 2,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 3,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 4,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 5,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 6,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 7,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 8,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 9,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 10,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 11,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 12,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 13,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 14,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 15,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 16,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 17,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 18,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 19,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 20,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 21,
                column: "current_period_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 22,
                column: "current_period_id",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "ix_branch_offices_current_period_id",
                table: "branch_offices",
                column: "current_period_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_payment_item_invoice_id",
                table: "invoice_payment_item",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_payment_item_payment_id",
                table: "invoice_payment_item",
                column: "payment_id");

            migrationBuilder.CreateIndex(
                name: "ix_payment_accounting_point_id",
                table: "payment",
                column: "accounting_point_id");

            migrationBuilder.AddForeignKey(
                name: "fk_branch_offices_period_current_period_id",
                table: "branch_offices",
                column: "current_period_id",
                principalTable: "period",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_branch_offices_period_current_period_id",
                table: "branch_offices");

            migrationBuilder.DropTable(
                name: "invoice_payment_item");

            migrationBuilder.DropTable(
                name: "payment");

            migrationBuilder.DropIndex(
                name: "ix_branch_offices_current_period_id",
                table: "branch_offices");

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "period",
                keyColumn: "id",
                keyValue: 18);

            migrationBuilder.DropColumn(
                name: "accounting_point_id",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "period_id",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "present_t1meter_reading",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "present_t2meter_reading",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "present_t3meter_reading",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "previous_t1meter_reading",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "previous_t2meter_reading",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "previous_t3meter_reading",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "t1sales",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "t1usage",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "t2sales",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "t2usage",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "t3sales",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "t3usage",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "total_amount_sales",
                table: "invoice");

            migrationBuilder.DropColumn(
                name: "current_period_id",
                table: "branch_offices");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "period",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<string>(
                name: "total_record",
                table: "payment_channels",
                type: "text",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "payment_channels",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AddColumn<int>(
                name: "consumption_t1",
                table: "invoice",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "consumption_t2",
                table: "invoice",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "consumption_t3",
                table: "invoice",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "paid_sum",
                table: "invoice",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "present_meter_reading_t1",
                table: "invoice",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "present_meter_reading_t2",
                table: "invoice",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "present_meter_reading_t3",
                table: "invoice",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "previous_meter_reading_t1",
                table: "invoice",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "previous_meter_reading_t2",
                table: "invoice",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "previous_meter_reading_t3",
                table: "invoice",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "sales_t1",
                table: "invoice",
                type: "decimal(8,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "sales_t2",
                table: "invoice",
                type: "decimal(8,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "sales_t3",
                table: "invoice",
                type: "decimal(8,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_sum",
                table: "invoice",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
