using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Erc.Households.Backend.DataAccess.PostgreSql.Migrations
{
    public partial class Add_addresses_persons_ac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "city",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    district_id = table.Column<int>(nullable: false),
                    is_district_city = table.Column<bool>(nullable: false),
                    is_region_city = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_city", x => x.id);
                    table.ForeignKey(
                        name: "FK_city_districts_district_id",
                        column: x => x.district_id,
                        principalTable: "districts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "street",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    city_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_street", x => x.id);
                    table.ForeignKey(
                        name: "FK_street_city_city_id",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    street_id = table.Column<int>(nullable: false),
                    building = table.Column<string>(maxLength: 20, nullable: false),
                    apt = table.Column<string>(maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.id);
                    table.ForeignKey(
                        name: "FK_address_street_street_id",
                        column: x => x.street_id,
                        principalTable: "street",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(maxLength: 50, nullable: false),
                    last_name = table.Column<string>(maxLength: 50, nullable: false),
                    patronymic = table.Column<string>(maxLength: 50, nullable: true),
                    tax_code = table.Column<string>(maxLength: 10, nullable: true),
                    id_card_number = table.Column<string>(maxLength: 9, nullable: true),
                    id_card_exp_date = table.Column<DateTime>(nullable: true),
                    address_id = table.Column<int>(nullable: true),
                    mobile_phone1 = table.Column<string>(maxLength: 15, nullable: true),
                    mobile_phone2 = table.Column<string>(maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.id);
                    table.ForeignKey(
                        name: "FK_person_address_address_id",
                        column: x => x.address_id,
                        principalTable: "address",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "accounting_point",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'10000000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 16, nullable: false),
                    eic = table.Column<string>(maxLength: 16, nullable: false),
                    address_id = table.Column<int>(nullable: false),
                    person_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounting_point", x => x.id);
                    table.ForeignKey(
                        name: "FK_accounting_point_address_address_id",
                        column: x => x.address_id,
                        principalTable: "address",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_accounting_point_person_person_id",
                        column: x => x.person_id,
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounting_point_address_id",
                table: "accounting_point",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_accounting_point_eic",
                table: "accounting_point",
                column: "eic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accounting_point_name",
                table: "accounting_point",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accounting_point_person_id",
                table: "accounting_point",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_address_street_id",
                table: "address",
                column: "street_id");

            migrationBuilder.CreateIndex(
                name: "IX_city_district_id",
                table: "city",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "IX_person_address_id",
                table: "person",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_person_id_card_number",
                table: "person",
                column: "id_card_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_person_tax_code",
                table: "person",
                column: "tax_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_street_city_id",
                table: "street",
                column: "city_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounting_point");

            migrationBuilder.DropTable(
                name: "person");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "street");

            migrationBuilder.DropTable(
                name: "city");
        }
    }
}
