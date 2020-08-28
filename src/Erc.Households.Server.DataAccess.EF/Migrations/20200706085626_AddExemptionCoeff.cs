using Microsoft.EntityFrameworkCore.Migrations;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class AddExemptionCoeff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_payments_payment_batches_batch_id",
                table: "payments");

            //migrationBuilder.DropColumn(
            //    name: "name",
            //    table: "payment_batches");

            migrationBuilder.AlterColumn<int>(
                name: "batch_id",
                table: "payments",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<decimal>(
                name: "exemption_coeff",
                table: "invoices",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_payment_batches_branch_office_id",
                table: "payment_batches",
                column: "branch_office_id");

            migrationBuilder.AddForeignKey(
                name: "fk_payment_batches_branch_offices_branch_office_id",
                table: "payment_batches",
                column: "branch_office_id",
                principalTable: "branch_offices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_payments_payment_batches_batch_id",
                table: "payments",
                column: "batch_id",
                principalTable: "payment_batches",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_payment_batches_branch_offices_branch_office_id",
                table: "payment_batches");

            migrationBuilder.DropForeignKey(
                name: "fk_payments_payment_batches_batch_id",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "ix_payment_batches_branch_office_id",
                table: "payment_batches");

            migrationBuilder.DropColumn(
                name: "exemption_coeff",
                table: "invoices");

            migrationBuilder.AlterColumn<int>(
                name: "batch_id",
                table: "payments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "payment_batches",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_payments_payment_batches_batch_id",
                table: "payments",
                column: "batch_id",
                principalTable: "payment_batches",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
