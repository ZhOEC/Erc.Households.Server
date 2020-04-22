using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Erc.Households.Server.DataAccess.EF.Migrations
{
    public partial class AddPaymentChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "invoice",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    from = table.Column<DateTime>(nullable: false),
                    to = table.Column<DateTime>(nullable: false),
                    previous_meter_reading_t1 = table.Column<int>(nullable: false),
                    present_meter_reading_t1 = table.Column<int>(nullable: false),
                    previous_meter_reading_t2 = table.Column<int>(nullable: false),
                    present_meter_reading_t2 = table.Column<int>(nullable: false),
                    previous_meter_reading_t3 = table.Column<int>(nullable: false),
                    present_meter_reading_t3 = table.Column<int>(nullable: false),
                    consumption_t1 = table.Column<int>(nullable: false),
                    consumption_t2 = table.Column<int>(nullable: false),
                    consumption_t3 = table.Column<int>(nullable: false),
                    sales_t1 = table.Column<decimal>(type: "decimal(8,5)", nullable: false),
                    sales_t2 = table.Column<decimal>(type: "decimal(8,5)", nullable: false),
                    sales_t3 = table.Column<decimal>(type: "decimal(8,5)", nullable: false),
                    total_sum = table.Column<decimal>(nullable: false),
                    paid_sum = table.Column<decimal>(nullable: false),
                    tariff_id = table.Column<int>(nullable: false),
                    note = table.Column<string>(nullable: true),
                    zone_record = table.Column<int>(nullable: false),
                    dso_consumption_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "tariffs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payment_channels",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    recordpoint_field_name = table.Column<string>(nullable: true),
                    sum_field_name = table.Column<string>(nullable: true),
                    date_field_name = table.Column<string>(nullable: true),
                    text_date_format = table.Column<string>(nullable: true),
                    person_field_name = table.Column<string>(nullable: true),
                    total_record = table.Column<string>(nullable: true),
                    type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payment_channels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "period",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_date = table.Column<DateTime>(nullable: false),
                    end_date = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_period", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "zone_coeffs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    zone_number = table.Column<int>(nullable: false),
                    zone_record = table.Column<int>(nullable: false),
                    value = table.Column<decimal>(nullable: false),
                    start_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_zone_coeffs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetail",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    invoice_id = table.Column<int>(nullable: false),
                    from = table.Column<DateTime>(nullable: false),
                    to = table.Column<DateTime>(nullable: false),
                    price_value = table.Column<decimal>(nullable: false),
                    consumption = table.Column<int>(nullable: false),
                    sales = table.Column<decimal>(nullable: false),
                    kz = table.Column<decimal>(nullable: false),
                    zone_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_detail", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_detail_invoice_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoice",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "zone_coeffs",
                columns: new[] { "id", "start_date", "value", "zone_number", "zone_record" },
                values: new object[,]
                {
                    { 1, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1m, 1, 1 },
                    { 2, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.5m, 1, 2 },
                    { 3, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1m, 2, 2 },
                    { 4, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.4m, 1, 3 },
                    { 5, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1m, 2, 3 },
                    { 6, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1.5m, 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "ix_invoice_tariff_id",
                table: "invoice",
                column: "tariff_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_detail_invoice_id",
                table: "InvoiceDetail",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_period_start_date",
                table: "period",
                column: "start_date",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceDetail");

            migrationBuilder.DropTable(
                name: "payment_channels");

            migrationBuilder.DropTable(
                name: "period");

            migrationBuilder.DropTable(
                name: "zone_coeffs");

            migrationBuilder.DropTable(
                name: "invoice");
        }
    }
}
