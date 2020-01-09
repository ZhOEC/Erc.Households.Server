using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Erc.Households.Backend.DataAccess.PostgreSql.Migrations
{
    public partial class Initial_create : Migration
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
                name: "regions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_regions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "districts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 200, nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_districts_region_id",
                table: "districts",
                column: "region_id");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "branch_offices");

            migrationBuilder.DropTable(
                name: "districts");

            migrationBuilder.DropTable(
                name: "regions");
        }
    }
}
