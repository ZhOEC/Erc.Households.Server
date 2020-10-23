using System;
using Erc.Households.Domain.Shared;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class taxinvoices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: -1);

            migrationBuilder.CreateTable(
                name: "tax_invoices",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'300000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lliability_date = table.Column<DateTime>(nullable: false),
                    lliability_sum = table.Column<decimal>(nullable: false),
                    energy_amount = table.Column<int>(nullable: false),
                    tariff_value = table.Column<decimal>(nullable: false),
                    tax_sum = table.Column<decimal>(nullable: false),
                    creation_date = table.Column<DateTime>(nullable: false),
                    operator_name = table.Column<string>(nullable: true),
                    full_sum = table.Column<decimal>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    branch_office_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tax_invoices", x => x.id);
                    table.ForeignKey(
                        name: "fk_tax_invoices_branch_offices_branch_office_id",
                        column: x => x.branch_office_id,
                        principalTable: "branch_offices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

           migrationBuilder.CreateIndex(
                name: "ix_tax_invoices_branch_office_id",
                table: "tax_invoices",
                column: "branch_office_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tax_invoices");

            migrationBuilder.DeleteData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 101);
        }
    }
}
