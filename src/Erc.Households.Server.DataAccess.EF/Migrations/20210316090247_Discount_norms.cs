using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Text.Json;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class Discount_norms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_accounting_point_exemption_accounting_points_accounting_poin",
                table: "accounting_point_exemptions");

            migrationBuilder.DropForeignKey(
                name: "fk_accounting_point_exemption_exemption_categories_exemption_c",
                table: "accounting_point_exemptions");

            migrationBuilder.DropForeignKey(
                name: "fk_accounting_point_exemption_people_person_id",
                table: "accounting_point_exemptions");

            migrationBuilder.DropForeignKey(
                name: "fk_accounting_point_tariff_accounting_points_accounting_point_id",
                table: "accounting_point_tariffs");

            migrationBuilder.DropForeignKey(
                name: "fk_accounting_point_tariff_tariffs_tariff_id",
                table: "accounting_point_tariffs");

            migrationBuilder.DropForeignKey(
                name: "fk_contract_accounting_points_accounting_point_id",
                table: "contracts");

            migrationBuilder.DropForeignKey(
                name: "fk_contract_people_customer_id",
                table: "contracts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_contract",
                table: "contracts");

            //migrationBuilder.DropIndex(
            //    name: "ix_accounting_points_name",
            //    table: "accounting_points");

            migrationBuilder.DropPrimaryKey(
                name: "pk_accounting_point_tariff",
                table: "accounting_point_tariffs");

            migrationBuilder.DropPrimaryKey(
                name: "pk_accounting_point_exemption",
                table: "accounting_point_exemptions");

            migrationBuilder.RenameIndex(
                name: "ix_contract_customer_id",
                table: "contracts",
                newName: "ix_contracts_customer_id");

            migrationBuilder.RenameIndex(
                name: "ix_accounting_point_tariff_tariff_id",
                table: "accounting_point_tariffs",
                newName: "ix_accounting_point_tariffs_tariff_id");

            migrationBuilder.RenameIndex(
                name: "ix_accounting_point_exemption_person_id",
                table: "accounting_point_exemptions",
                newName: "ix_accounting_point_exemptions_person_id");

            migrationBuilder.RenameIndex(
                name: "ix_accounting_point_exemption_exemption_category_id",
                table: "accounting_point_exemptions",
                newName: "ix_accounting_point_exemptions_exemption_category_id");

            migrationBuilder.AddColumn<bool>(
                name: "is_centralized_hot_water_supply",
                table: "accounting_points",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_centralized_water_supply",
                table: "accounting_points",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_gas_water_heater_installed",
                table: "accounting_points",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_contracts",
                table: "contracts",
                columns: new[] { "accounting_point_id", "id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_accounting_point_tariffs",
                table: "accounting_point_tariffs",
                columns: new[] { "accounting_point_id", "id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_accounting_point_exemptions",
                table: "accounting_point_exemptions",
                columns: new[] { "accounting_point_id", "id" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 1,
                column: "iban",
                value: "UA773116470000026038318392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 2,
                column: "iban",
                value: "UA353116470000026036309392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 3,
                column: "iban",
                value: "UA453116470000026038307392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 4,
                column: "iban",
                value: "UA253116470000026031304392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 5,
                column: "iban",
                value: "UA823116470000026039317392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 6,
                column: "iban",
                value: "UA623116470000026032314392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 7,
                column: "iban",
                value: "UA503116470000026039306392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 8,
                column: "iban",
                value: "UA523116470000026030316392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 9,
                column: "iban",
                value: "UA403116470000026034301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 10,
                column: "iban",
                value: "UA173116470000026036321392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 11,
                column: "iban",
                value: "UA073116470000026034323392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 12,
                column: "iban",
                value: "UA353116470000026033302392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 13,
                column: "iban",
                value: "UA573116470000026031315392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 14,
                column: "iban",
                value: "UA203116470000026030305392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 15,
                column: "iban",
                value: "UA303116470000026032303392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 16,
                column: "iban",
                value: "UA123116470000026035322392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 17,
                column: "iban",
                value: "UA823116470000026036310392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 18,
                column: "iban",
                value: "UA723116470000026037319392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 19,
                column: "iban",
                value: "UA723116470000026034312392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 20,
                column: "iban",
                value: "UA673116470000026033313392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 21,
                column: "iban",
                value: "UA403116470000026037308392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 22,
                column: "iban",
                value: "UA773116470000026035311392990");

            //migrationBuilder.CreateIndex(
            //    name: "ix_accounting_points_name_branch_office_id",
            //    table: "accounting_points",
            //    columns: new[] { "name", "branch_office_id" },
            //    unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_accounting_point_exemptions_accounting_points_accounting_po",
                table: "accounting_point_exemptions",
                column: "accounting_point_id",
                principalTable: "accounting_points",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_accounting_point_exemptions_exemption_categories_exemption_",
                table: "accounting_point_exemptions",
                column: "exemption_category_id",
                principalTable: "exemption_categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_accounting_point_exemptions_people_person_id",
                table: "accounting_point_exemptions",
                column: "person_id",
                principalTable: "people",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_accounting_point_tariffs_accounting_points_accounting_point",
                table: "accounting_point_tariffs",
                column: "accounting_point_id",
                principalTable: "accounting_points",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_accounting_point_tariffs_tariffs_tariff_id",
                table: "accounting_point_tariffs",
                column: "tariff_id",
                principalTable: "tariffs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_contracts_accounting_points_accounting_point_id",
                table: "contracts",
                column: "accounting_point_id",
                principalTable: "accounting_points",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_contracts_people_customer_id",
                table: "contracts",
                column: "customer_id",
                principalTable: "people",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.UpdateData(
                table: "usage_categories",
                keyColumn: "id",
                keyValue: 1,
                column: "exemption_discount_norms",
                value: JsonSerializer.Serialize(new[] { new { EffectiveDate = new DateTime(2019, 1, 1), BaseUnits = 70, BaseUnitsWithoutHotWater = 100, BasePerson = 1, UnitsPerPerson = 30, MaxUnits = 190, MaxUnitsWithoutHotWater = 220 } }));

            migrationBuilder.UpdateData(
                table: "usage_categories",
                keyColumn: "id",
                keyValue: 2,
                column: "exemption_discount_norms",
                value: JsonSerializer.Serialize(new[] { new { EffectiveDate = new DateTime(2019, 1, 1), BaseUnits = 110, BaseUnitsWithoutHotWater = 130, BasePerson = 1, UnitsPerPerson = 30, MaxUnits = 230, MaxUnitsWithoutHotWater = 250 } }));

            migrationBuilder.UpdateData(
                table: "usage_categories",
                keyColumn: "id",
                keyValue: 3,
                column: "exemption_discount_norms",
                value: JsonSerializer.Serialize(new[] { new { EffectiveDate = new DateTime(2019, 1, 1), BaseUnits = 70, BaseUnitsWithoutHotWater = 100, BasePerson = 1, UnitsPerPerson = 30, MaxUnits = 190, MaxUnitsWithoutHotWater = 220, BaseSquareMeter = 10.5m, SquareMeterPerPerson = 21m, UnitsPerSquareMeter = 30 } }));

            migrationBuilder.UpdateData(
                table: "usage_categories",
                keyColumn: "id",
                keyValue: 4,
                column: "exemption_discount_norms",
                value: JsonSerializer.Serialize(new[] { new { EffectiveDate = new DateTime(2019, 1, 1), BaseUnits = 110, BaseUnitsWithoutHotWater = 130, BasePerson = 1, UnitsPerPerson = 30, MaxUnits = 230, MaxUnitsWithoutHotWater = 250, BaseSquareMeter = 10.5m, SquareMeterPerPerson = 21m, UnitsPerSquareMeter = 30 } }));
            /* initial data
                  { 4, JsonSerializer.Serialize(new[] { new { EffectiveDate = new DateTime(2019, 1, 1), BaseKWh = 110, BaseKWhWithoutHotWater = 130, BasePerson = 1, KWhPerPerson = 30, MaxKWh = 230, MaxKWhWithoutHotWater = 250, BaseSquareMeter = 10.5m, SquareMeterPerPerson = 21m, KWhPerSquareMeter = 30 } }), "Електроопалювальна установка та електроплита" },
                  { 3, JsonSerializer.Serialize(new[] { new { EffectiveDate = new DateTime(2019, 1, 1), BaseKWh = 70, BaseKWhWithoutHotWater = 100, BasePerson = 1, KWhPerPerson = 30, MaxKWh = 190, MaxKWhWithoutHotWater = 220, BaseSquareMeter = 10.5m, SquareMeterPerPerson = 21m, KWhPerSquareMeter = 30 } }), "Електроопалювальна установка" },
                  { 2, JsonSerializer.Serialize(new[] { new { EffectiveDate = new DateTime(2019, 1, 1), BaseKWh = 110, BaseKWhWithoutHotWater = 130, BasePerson = 1, KWhPerPerson = 30, MaxKWh = 230, MaxKWhWithoutHotWater = 250 } }), "Електроплита" },
                  { 1,  JsonSerializer.Serialize(new[] { new { EffectiveDate = new DateTime(2019, 1, 1), BaseKWh = 70, BaseKWhWithoutHotWater = 100, BasePerson = 1, KWhPerPerson = 30, MaxKWh = 190, MaxKWhWithoutHotWater = 220 } }) , "Звичайне споживання" }
                 */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_accounting_point_exemptions_accounting_points_accounting_po",
                table: "accounting_point_exemptions");

            migrationBuilder.DropForeignKey(
                name: "fk_accounting_point_exemptions_exemption_categories_exemption_",
                table: "accounting_point_exemptions");

            migrationBuilder.DropForeignKey(
                name: "fk_accounting_point_exemptions_people_person_id",
                table: "accounting_point_exemptions");

            migrationBuilder.DropForeignKey(
                name: "fk_accounting_point_tariffs_accounting_points_accounting_point",
                table: "accounting_point_tariffs");

            migrationBuilder.DropForeignKey(
                name: "fk_accounting_point_tariffs_tariffs_tariff_id",
                table: "accounting_point_tariffs");

            migrationBuilder.DropForeignKey(
                name: "fk_contracts_accounting_points_accounting_point_id",
                table: "contracts");

            migrationBuilder.DropForeignKey(
                name: "fk_contracts_people_customer_id",
                table: "contracts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_contracts",
                table: "contracts");

            migrationBuilder.DropIndex(
                name: "ix_accounting_points_name_branch_office_id",
                table: "accounting_points");

            migrationBuilder.DropPrimaryKey(
                name: "pk_accounting_point_tariffs",
                table: "accounting_point_tariffs");

            migrationBuilder.DropPrimaryKey(
                name: "pk_accounting_point_exemptions",
                table: "accounting_point_exemptions");

            migrationBuilder.DropColumn(
                name: "is_centralized_hot_water_supply",
                table: "accounting_points");

            migrationBuilder.DropColumn(
                name: "is_centralized_water_supply",
                table: "accounting_points");

            migrationBuilder.DropColumn(
                name: "is_gas_water_heater_installed",
                table: "accounting_points");

            migrationBuilder.RenameIndex(
                name: "ix_contracts_customer_id",
                table: "contracts",
                newName: "ix_contract_customer_id");

            migrationBuilder.RenameIndex(
                name: "ix_accounting_point_tariffs_tariff_id",
                table: "accounting_point_tariffs",
                newName: "ix_accounting_point_tariff_tariff_id");

            migrationBuilder.RenameIndex(
                name: "ix_accounting_point_exemptions_person_id",
                table: "accounting_point_exemptions",
                newName: "ix_accounting_point_exemption_person_id");

            migrationBuilder.RenameIndex(
                name: "ix_accounting_point_exemptions_exemption_category_id",
                table: "accounting_point_exemptions",
                newName: "ix_accounting_point_exemption_exemption_category_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_contract",
                table: "contracts",
                columns: new[] { "accounting_point_id", "id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_accounting_point_tariff",
                table: "accounting_point_tariffs",
                columns: new[] { "accounting_point_id", "id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_accounting_point_exemption",
                table: "accounting_point_exemptions",
                columns: new[] { "accounting_point_id", "id" });

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 1,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 2,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 3,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 4,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 5,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 6,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 7,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 8,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 9,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 10,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 11,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 12,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 13,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 14,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 15,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 16,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 17,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 18,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 19,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 20,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 21,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.UpdateData(
                table: "branch_offices",
                keyColumn: "id",
                keyValue: 22,
                column: "iban",
                value: "UA703116470000026001301392990");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_name",
                table: "accounting_points",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_accounting_point_exemption_accounting_points_accounting_poin",
                table: "accounting_point_exemptions",
                column: "accounting_point_id",
                principalTable: "accounting_points",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_accounting_point_exemption_exemption_categories_exemption_c",
                table: "accounting_point_exemptions",
                column: "exemption_category_id",
                principalTable: "exemption_categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_accounting_point_exemption_people_person_id",
                table: "accounting_point_exemptions",
                column: "person_id",
                principalTable: "people",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_accounting_point_tariff_accounting_points_accounting_point_id",
                table: "accounting_point_tariffs",
                column: "accounting_point_id",
                principalTable: "accounting_points",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_accounting_point_tariff_tariffs_tariff_id",
                table: "accounting_point_tariffs",
                column: "tariff_id",
                principalTable: "tariffs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_contract_accounting_points_accounting_point_id",
                table: "contracts",
                column: "accounting_point_id",
                principalTable: "accounting_points",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_contract_people_customer_id",
                table: "contracts",
                column: "customer_id",
                principalTable: "people",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
