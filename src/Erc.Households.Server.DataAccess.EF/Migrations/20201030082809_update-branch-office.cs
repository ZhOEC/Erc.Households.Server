using Microsoft.EntityFrameworkCore.Migrations;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class updatebranchoffice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "account_number",
                table: "branch_offices");

            migrationBuilder.AddColumn<string>(
                name: "bank_full_name",
                table: "branch_offices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "iban",
                table: "branch_offices",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 101,
                columns: new[] { "bank_full_name", "iban" },
                values: new object[] { "філія Житомирського ОУ АТ «Ощадбанк»", "UA703116470000026001301392990" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bank_full_name",
                table: "branch_offices");

            migrationBuilder.DropColumn(
                name: "iban",
                table: "branch_offices");

            migrationBuilder.AddColumn<string>(
                name: "account_number",
                table: "branch_offices",
                type: "text",
                nullable: true);
        }
    }
}
