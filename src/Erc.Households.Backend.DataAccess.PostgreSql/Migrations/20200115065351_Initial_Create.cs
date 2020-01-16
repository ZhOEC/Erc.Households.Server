using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Erc.Households.Backend.DataAccess.PostgreSql.Migrations
{
    public partial class Initial_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "branch_offices",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    string_id = table.Column<string>(maxLength: 2, nullable: false),
                    address = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branch_offices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "distribution_system_operators",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_distribution_system_operators", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "regions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_regions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tariffs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tariffs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "districts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                    region_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_districts", x => x.id);
                    table.ForeignKey(
                        name: "FK_districts_regions_region_id",
                        column: x => x.region_id,
                        principalTable: "regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tariff_rates",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_date = table.Column<DateTime>(nullable: false),
                    value = table.Column<decimal>(type: "decimal(8,5)", nullable: false),
                    consumption_limit = table.Column<int>(nullable: true),
                    heating_consumption_limit = table.Column<int>(nullable: true),
                    heating_start_day = table.Column<DateTime>(nullable: true),
                    heating_end_day = table.Column<DateTime>(nullable: true),
                    tariff_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tariff_rates", x => x.id);
                    table.ForeignKey(
                        name: "FK_tariff_rates_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "tariffs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "city",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 100, nullable: false),
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
                    name = table.Column<string>(maxLength: 100, nullable: false),
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
                name: "accounting_points",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'10000000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 16, nullable: false),
                    eic = table.Column<string>(maxLength: 16, nullable: false),
                    address_id = table.Column<int>(nullable: false),
                    owner_id = table.Column<int>(nullable: false),
                    tariff_id = table.Column<int>(nullable: false),
                    distribution_system_operator_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounting_points", x => x.id);
                    table.ForeignKey(
                        name: "FK_accounting_points_address_address_id",
                        column: x => x.address_id,
                        principalTable: "address",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_accounting_points_distribution_system_operators_distributio~",
                        column: x => x.distribution_system_operator_id,
                        principalTable: "distribution_system_operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_accounting_points_person_owner_id",
                        column: x => x.owner_id,
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_accounting_points_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "tariffs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounting_points_address_id",
                table: "accounting_points",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_accounting_points_distribution_system_operator_id",
                table: "accounting_points",
                column: "distribution_system_operator_id");

            migrationBuilder.CreateIndex(
                name: "IX_accounting_points_eic",
                table: "accounting_points",
                column: "eic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accounting_points_name",
                table: "accounting_points",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accounting_points_owner_id",
                table: "accounting_points",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_accounting_points_tariff_id",
                table: "accounting_points",
                column: "tariff_id");

            migrationBuilder.CreateIndex(
                name: "IX_address_street_id",
                table: "address",
                column: "street_id");

            migrationBuilder.CreateIndex(
                name: "IX_city_district_id",
                table: "city",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "IX_districts_region_id",
                table: "districts",
                column: "region_id");

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

            migrationBuilder.CreateIndex(
                name: "IX_tariff_rates_tariff_id",
                table: "tariff_rates",
                column: "tariff_id");

            migrationBuilder.Sql(@"insert into branch_offices(name, string_id, address) values ('Андрушівський ЦОК', 'an', '')
                                , ('Баранiвський ЦОК', 'bn', '')
                                , ('Бердичiвський ЦОК', 'bd', '')
                                , ('Брусилівський ЦОК', 'br', '')
                                , ('Хорошівський ЦОК', 'hr', '')
                                , ('Ємільчинський ЦОК', 'em', '')
                                , ('Житомирський ЦОК', 'zt', '')
                                , ('Зарічанський ЦОК', 'zr', '')
                                , ('Коростенський ЦОК', 'kr', '')
                                , ('Коростишiвський ЦОК', 'kt', '')
                                , ('Любарський ЦОК', 'lb', '')
                                , ('Малинський ЦОК', 'ml', '')
                                , ('Народицький ЦОК', 'nr', '')
                                , ('Новоград-Волинський ЦОК', 'nv', '')
                                , ('Овруцький ЦОК', 'ov', '')
                                , ('Олевський ЦОК', 'ol', '')
                                , ('Попільнянський ЦОК', 'pp', '')
                                , ('Радомишльський ЦОК', 'rd', '')
                                , ('Романівський ЦОК', 'rm', '')
                                , ('Пулинський ЦОК', 'pl', '')
                                , ('Черняхівський ЦОК', 'ch', '')
                                , ('Чуднівський ЦОК', 'cd', '')");

            migrationBuilder.Sql(@"insert into regions(name) values ('Житомирська обл.')");

            migrationBuilder.Sql(@"insert into districts(name, region_id) values ('Андрушівський р-н', 1)
                                , ('Баранiвський р-н', 1)
                                , ('Бердичiвський р-н', 1)
                                , ('Брусилівський р-н', 1)
                                , ('Хорошівський р-н', 1)
                                , ('Ємільчинський р-н', 1)
                                , ('Житомирський р-н', 1)
                                , ('Зарічанський р-н', 1)
                                , ('Коростенський р-н', 1)
                                , ('Коростишiвський р-н', 1)
				                , ('Лугинський р-н', 1)
                                , ('Любарський р-н', 1)
                                , ('Малинський р-н', 1)
                                , ('Народицький р-н', 1)
                                , ('Новоград-Волинський р-н', 1)
                                , ('Овруцький р-н', 1)
                                , ('Олевський р-н', 1)
                                , ('Попільнянський р-н', 1)
                                , ('Радомишльський р-н', 1)
                                , ('Романівський р-н', 1)
                                , ('Ружинський р-н', 1)
                                , ('Пулинський р-н', 1)
                                , ('Черняхівський р-н', 1)
                                , ('Чуднівський р-н', 1)");

            migrationBuilder.Sql(@"insert into distribution_system_operators(name) values ('АТ «Житомиробленерго»'), ('АТ «Укрзалізниця»')");

            migrationBuilder.Sql(@"insert into tariffs(name) values ('Населення (загальний тариф)')
                                    , ('Будинки з електроопалювальними установками')
                                    , ('Багатоквартирні негазифіковані будинки')
                                    , ('Багатодітні, прийомні сім''ї та дитячі будинкі сімейного типу')");

            migrationBuilder.Sql(@"insert into tariff_rates(start_date, value, consumption_limit, heating_consumption_limit, heating_start_day, heating_end_day, tariff_id) 
                        values ('2017-03-01', 0.9, 100, NULL, NULL, NULL, 1)
                        ,('2017-03-01', 1.68000, NULL, NULL, NULL, NULL, 1)
                        ,('2017-03-01', 0.90000, 100, 3000, '2019-10-01','2020-04-30',2)
                        ,('2017-03-01', 1.68000, NULL, NULL, NULL, NULL, 2)
                        ,('2017-03-01', 0.90000, 100, 3000, '2019-10-01', '2020-04-30',3)
                        ,('2017-03-01', 1.68000, NULL, NULL, NULL, NULL, 3)
                        ,('2017-03-01', 0.90000, NULL, NULL, NULL, NULL, 4)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounting_points");

            migrationBuilder.DropTable(
                name: "branch_offices");

            migrationBuilder.DropTable(
                name: "tariff_rates");

            migrationBuilder.DropTable(
                name: "distribution_system_operators");

            migrationBuilder.DropTable(
                name: "person");

            migrationBuilder.DropTable(
                name: "tariffs");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "street");

            migrationBuilder.DropTable(
                name: "city");

            migrationBuilder.DropTable(
                name: "districts");

            migrationBuilder.DropTable(
                name: "regions");
        }
    }
}
