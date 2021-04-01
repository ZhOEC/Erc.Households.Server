using Microsoft.EntityFrameworkCore.Migrations;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class addbranchofficeprivatbankid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "privat_bank_id",
                table: "branch_offices",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 1,
                column: "privat_bank_id",
                value: 3453332);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 2,
                column: "privat_bank_id",
                value: 3453416);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 3,
                column: "privat_bank_id",
                value: 3453439);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 4,
                column: "privat_bank_id",
                value: 3453474);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 5,
                column: "privat_bank_id",
                value: 3453501);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 6,
                column: "privat_bank_id",
                value: 3453542);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 7,
                column: "privat_bank_id",
                value: 3453596);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 8,
                column: "privat_bank_id",
                value: 3453569);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 9,
                column: "privat_bank_id",
                value: 3453621);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 10,
                column: "privat_bank_id",
                value: 3453713);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 11,
                column: "privat_bank_id",
                value: 3453751);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 12,
                column: "privat_bank_id",
                value: 3453767);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 13,
                column: "privat_bank_id",
                value: 3453796);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 14,
                column: "privat_bank_id",
                value: 3453858);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 15,
                column: "privat_bank_id",
                value: 3453897);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 16,
                column: "privat_bank_id",
                value: 3453913);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 17,
                column: "privat_bank_id",
                value: 3453927);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 18,
                column: "privat_bank_id",
                value: 3453950);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 19,
                column: "privat_bank_id",
                value: 3453965);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 20,
                column: "privat_bank_id",
                value: 3453981);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 21,
                column: "privat_bank_id",
                value: 3454001);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 22,
                column: "privat_bank_id",
                value: 3454038);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "privat_bank_id",
                table: "branch_offices");
        }
    }
}
