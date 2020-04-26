using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Erc.Households.Server.DataAccess.EF.Migrations
{
    public partial class PaymentChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "payment_channels",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", nullable: false),
                    recordpoint_field_name = table.Column<string>(nullable: true),
                    sum_field_name = table.Column<string>(nullable: true),
                    date_field_name = table.Column<string>(nullable: true),
                    text_date_format = table.Column<string>(nullable: true),
                    person_field_name = table.Column<string>(nullable: true),
                    total_record = table.Column<int>(nullable: false),
                    type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payment_channels", x => x.id);
                });
        }
    }
}
