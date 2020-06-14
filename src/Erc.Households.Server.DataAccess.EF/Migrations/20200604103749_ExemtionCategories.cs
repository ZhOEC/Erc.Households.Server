using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class ExemtionCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_contract",
                table: "contracts");

            migrationBuilder.DropIndex(
                name: "ix_contract_accounting_point_id",
                table: "contracts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_accounting_point_tariff",
                table: "accounting_point_tariffs");

            migrationBuilder.DropIndex(
                name: "ix_accounting_point_tariff_accounting_point_id",
                table: "accounting_point_tariffs");

            migrationBuilder.AddPrimaryKey(
                name: "pk_contract",
                table: "contracts",
                columns: new[] { "accounting_point_id", "id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_accounting_point_tariff",
                table: "accounting_point_tariffs",
                columns: new[] { "accounting_point_id", "id" });

            migrationBuilder.CreateTable(
                name: "exemption_categories",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    coeff = table.Column<decimal>(nullable: false),
                    has_limit = table.Column<bool>(nullable: true),
                    effective_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exemption_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "accounting_point_exemption",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accounting_point_id = table.Column<int>(nullable: false),
                    exemption_category_id = table.Column<int>(nullable: false),
                    person_id = table.Column<int>(nullable: false),
                    effective_date = table.Column<DateTime>(nullable: false),
                    end_date = table.Column<DateTime>(nullable: true),
                    certificate = table.Column<string>(nullable: true),
                    has_limit = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounting_point_exemption", x => x.id);
                    table.ForeignKey(
                        name: "fk_accounting_point_exemption_accounting_points_accounting_poin",
                        column: x => x.accounting_point_id,
                        principalTable: "accounting_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_point_exemption_exemption_categories_exemption_c",
                        column: x => x.exemption_category_id,
                        principalTable: "exemption_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_point_exemption_people_person_id",
                        column: x => x.person_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "exemption_categories",
                columns: new[] { "id", "coeff", "effective_date", "end_date", "has_limit", "name" },
                values: new object[,]
                {
                    { 1, 100.0m, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Почесні громадяни міста" },
                    { 2, 50.0m, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Iнвалiди 1 групи по зору або з ураженням ОРА" },
                    { 3, 50.0m, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Iнвалiди 2 групи по зору або з ураженням ОРА" },
                    { 4, 100.0m, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Учасник бойових дій та членів родин загиблих в АТО(ООС) (Місцевий бюджет)" }
                });

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202001,
                column: "name",
                value: "Січень 2020р.");

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202002,
                column: "name",
                value: "Лютий 2020р.");

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202003,
                column: "name",
                value: "Березень 2020р.");

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202004,
                column: "name",
                value: "Квітень 2020р.");

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202005,
                column: "name",
                value: "Травень 2020р.");

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202006,
                column: "name",
                value: "Червень 2020р.");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_point_exemption_accounting_point_id",
                table: "accounting_point_exemption",
                column: "accounting_point_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_point_exemption_exemption_category_id",
                table: "accounting_point_exemption",
                column: "exemption_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_point_exemption_person_id",
                table: "accounting_point_exemption",
                column: "person_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounting_point_exemption");

            migrationBuilder.DropTable(
                name: "exemption_categories");

            migrationBuilder.DropPrimaryKey(
                name: "pk_contract",
                table: "contracts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_accounting_point_tariff",
                table: "accounting_point_tariffs");

            migrationBuilder.AddPrimaryKey(
                name: "pk_contract",
                table: "contracts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_accounting_point_tariff",
                table: "accounting_point_tariffs",
                column: "id");

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202001,
                column: "name",
                value: "Січень 2019р.");

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202002,
                column: "name",
                value: "Лютий 2019р.");

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202003,
                column: "name",
                value: "Березень 2019р.");

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202004,
                column: "name",
                value: "Квітень 2019р.");

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202005,
                column: "name",
                value: "Травень 2019р.");

            migrationBuilder.UpdateData(
                table: "periods",
                keyColumn: "id",
                keyValue: 202006,
                column: "name",
                value: "Червень 2019р.");

            migrationBuilder.CreateIndex(
                name: "ix_contract_accounting_point_id",
                table: "contracts",
                column: "accounting_point_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_point_tariff_accounting_point_id",
                table: "accounting_point_tariffs",
                column: "accounting_point_id");
        }
    }
}
