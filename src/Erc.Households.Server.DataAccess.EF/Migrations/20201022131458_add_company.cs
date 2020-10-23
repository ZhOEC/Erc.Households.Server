using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class add_company : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "operator_name",
                table: "tax_invoices");

            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "branch_offices",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "company",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    address = table.Column<string>(nullable: true),
                    taxpayer_phone = table.Column<string>(nullable: true),
                    state_registry_code = table.Column<string>(nullable: true),
                    taxpayer_number = table.Column<string>(nullable: true),
                    bookkeeper_name = table.Column<string>(nullable: true),
                    bookkeeper_tax_number = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "company",
                columns: new[] { "id", "address", "bookkeeper_name", "bookkeeper_tax_number", "state_registry_code", "taxpayer_number", "taxpayer_phone" },
                values: new object[] { 1, "10003, майдан Перемоги, буд. 10 м. Житомир", "А. В. Івчук", "2778207983", "42095943", "420959406258", "0412402109" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 1,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 2,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 3,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 4,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 5,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 6,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 7,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 8,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 9,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 10,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 11,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 12,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 13,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 14,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 15,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 16,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 17,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 18,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 19,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 20,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 21,
                column: "company_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 22,
                column: "company_id",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "ix_branch_offices_company_id",
                table: "branch_offices",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "fk_branch_offices_company_company_id",
                table: "branch_offices",
                column: "company_id",
                principalTable: "company",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_branch_offices_company_company_id",
                table: "branch_offices");

            migrationBuilder.DropTable(
                name: "company");

            migrationBuilder.DropIndex(
                name: "ix_branch_offices_company_id",
                table: "branch_offices");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "branch_offices");

            migrationBuilder.AddColumn<string>(
                name: "operator_name",
                table: "tax_invoices",
                type: "text",
                nullable: true);
        }
    }
}
