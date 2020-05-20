using Microsoft.EntityFrameworkCore.Migrations;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class UpdateTableFields3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_payment_batches_payment_channel_id",
                table: "payment_batches",
                column: "payment_channel_id");

            migrationBuilder.AddForeignKey(
                name: "fk_payment_batches_payment_channels_payment_channel_id",
                table: "payment_batches",
                column: "payment_channel_id",
                principalTable: "payment_channels",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_payment_batches_payment_channels_payment_channel_id",
                table: "payment_batches");

            migrationBuilder.DropIndex(
                name: "ix_payment_batches_payment_channel_id",
                table: "payment_batches");
        }
    }
}
