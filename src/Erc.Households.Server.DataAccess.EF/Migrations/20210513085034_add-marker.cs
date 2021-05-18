using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class addmarker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "markers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "citext", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_markers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "accounting_point_markers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accounting_point_id = table.Column<int>(type: "integer", nullable: false),
                    marker_id = table.Column<int>(type: "integer", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounting_point_markers", x => new { x.accounting_point_id, x.id });
                    table.ForeignKey(
                        name: "fk_accounting_point_markers_accounting_points_accounting_point",
                        column: x => x.accounting_point_id,
                        principalTable: "accounting_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_point_markers_markers_marker_id",
                        column: x => x.marker_id,
                        principalTable: "markers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_accounting_point_markers_marker_id",
                table: "accounting_point_markers",
                column: "marker_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounting_point_markers");

            migrationBuilder.DropTable(
                name: "markers");
        }
    }
}
