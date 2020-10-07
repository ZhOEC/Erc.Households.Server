using System;
using System.Collections.Generic;
using Erc.Households.CalculateStrategies.Core;
using Erc.Households.Domain.Billing;
using Erc.Households.Domain.Shared;
using Erc.Households.ModelLogs;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Erc.Households.EF.PostgreSQL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:commodity", "electric_power,natural_gas")
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.CreateTable(
                name: "accounting_point_debt_history",
                columns: table => new
                {
                    accounting_point_id = table.Column<int>(nullable: false),
                    period_id = table.Column<int>(nullable: false),
                    debt_value = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounting_point_debt_history", x => new { x.accounting_point_id, x.period_id });
                });

            migrationBuilder.CreateTable(
                name: "building_types",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    heataing_correction = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_building_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "distribution_system_operators",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    commodity = table.Column<Commodity>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_distribution_system_operators", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "exemption_categories",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    coeff = table.Column<decimal>(nullable: false),
                    has_limit = table.Column<bool>(nullable: true),
                    effective_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exemption_categories", x => x.id);
                });

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
                    payments_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payment_channels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "periods",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    name = table.Column<string>(type: "citext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_periods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "regions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_regions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tariffs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", maxLength: 200, nullable: true),
                    commodity = table.Column<Commodity>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tariffs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "usage_categories",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    exemption_discount_norms = table.Column<IEnumerable<ExemptionDiscountNorms>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usage_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "zone_coeffs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    zone_number = table.Column<int>(nullable: false),
                    zone_record = table.Column<int>(nullable: false),
                    value = table.Column<decimal>(nullable: false),
                    discount_weight = table.Column<decimal>(nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_zone_coeffs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "branch_offices",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", maxLength: 200, nullable: false),
                    string_id = table.Column<string>(type: "citext", maxLength: 2, nullable: false),
                    district_ids = table.Column<int[]>(nullable: true),
                    address = table.Column<string>(type: "citext", maxLength: 500, nullable: false),
                    available_commodities = table.Column<Commodity[]>(nullable: true),
                    current_period_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_branch_offices", x => x.id);
                    table.ForeignKey(
                        name: "fk_branch_offices_periods_current_period_id",
                        column: x => x.current_period_id,
                        principalTable: "periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "districts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    region_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_districts", x => x.id);
                    table.ForeignKey(
                        name: "fk_districts_regions_region_id",
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
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    value = table.Column<decimal>(type: "decimal(8,5)", nullable: false),
                    consumption_limit = table.Column<int>(nullable: true),
                    heating_consumption_limit = table.Column<int>(nullable: true),
                    heating_start_day = table.Column<DateTime>(nullable: true),
                    heating_end_day = table.Column<DateTime>(nullable: true),
                    tariff_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tariff_rate", x => x.id);
                    table.ForeignKey(
                        name: "fk_tariff_rate_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "tariffs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payment_batches",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    incoming_date = table.Column<DateTime>(type: "date", nullable: false),
                    branch_office_id = table.Column<int>(nullable: false),
                    payment_channel_id = table.Column<int>(nullable: false),
                    is_closed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payment_batches", x => x.id);
                    table.ForeignKey(
                        name: "fk_payment_batches_branch_offices_branch_office_id",
                        column: x => x.branch_office_id,
                        principalTable: "branch_offices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_payment_batches_payment_channels_payment_channel_id",
                        column: x => x.payment_channel_id,
                        principalTable: "payment_channels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    district_id = table.Column<int>(nullable: false),
                    is_district_city = table.Column<bool>(nullable: false, defaultValue: false),
                    is_region_city = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cities", x => x.id);
                    table.ForeignKey(
                        name: "fk_cities_districts_district_id",
                        column: x => x.district_id,
                        principalTable: "districts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "streets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    city_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_streets", x => x.id);
                    table.ForeignKey(
                        name: "fk_streets_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    zip = table.Column<string>(nullable: true),
                    street_id = table.Column<int>(nullable: false),
                    building = table.Column<string>(type: "citext", maxLength: 10, nullable: false),
                    apt = table.Column<string>(type: "citext", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_addresses", x => x.id);
                    table.CheckConstraint("ck_address_zip", "zip ~ '^(\\d){5}$'");
                    table.ForeignKey(
                        name: "fk_addresses_streets_street_id",
                        column: x => x.street_id,
                        principalTable: "streets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "people",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "citext", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "citext", maxLength: 50, nullable: false),
                    patronymic = table.Column<string>(type: "citext", maxLength: 50, nullable: true),
                    tax_code = table.Column<string>(maxLength: 10, nullable: true),
                    id_card_number = table.Column<string>(type: "citext", maxLength: 9, nullable: false),
                    id_card_issuance_date = table.Column<DateTime>(nullable: false),
                    id_card_issuer = table.Column<string>(type: "citext", nullable: true),
                    id_card_exp_date = table.Column<DateTime>(nullable: true),
                    address_id = table.Column<int>(nullable: true),
                    mobile_phones = table.Column<string[]>(type: "varchar(15)[]", nullable: true),
                    email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_people", x => x.id);
                    table.ForeignKey(
                        name: "fk_people_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
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
                    name = table.Column<string>(type: "citext", maxLength: 16, nullable: false),
                    eic = table.Column<string>(type: "citext", maxLength: 16, nullable: false),
                    address_id = table.Column<int>(nullable: false),
                    owner_id = table.Column<int>(nullable: false),
                    distribution_system_operator_id = table.Column<int>(nullable: false),
                    branch_office_id = table.Column<int>(nullable: false),
                    debt = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    zone_record = table.Column<int>(nullable: false),
                    commodity = table.Column<Commodity>(nullable: false, defaultValue: Commodity.ElectricPower),
                    usage_category_id = table.Column<int>(nullable: false),
                    building_type_id = table.Column<int>(nullable: false),
                    contract_is_signed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounting_points", x => x.id);
                    table.CheckConstraint("CK_accounting_point_eic", "length(eic) = 16");
                    table.ForeignKey(
                        name: "fk_accounting_points_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_points_branch_offices_branch_office_id",
                        column: x => x.branch_office_id,
                        principalTable: "branch_offices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_points_building_types_building_type_id",
                        column: x => x.building_type_id,
                        principalTable: "building_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_points_distribution_system_operators_distributio",
                        column: x => x.distribution_system_operator_id,
                        principalTable: "distribution_system_operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_points_people_owner_id",
                        column: x => x.owner_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_points_usage_categories_usage_category_id",
                        column: x => x.usage_category_id,
                        principalTable: "usage_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accounting_point_exemptions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accounting_point_id = table.Column<int>(nullable: false),
                    exemption_category_id = table.Column<int>(nullable: false),
                    person_id = table.Column<int>(nullable: false),
                    effective_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: true),
                    certificate = table.Column<string>(nullable: true),
                    persons_number = table.Column<int>(nullable: false),
                    has_limit = table.Column<bool>(nullable: false, defaultValue: true),
                    note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounting_point_exemption", x => new { x.accounting_point_id, x.id });
                    table.ForeignKey(
                        name: "fk_accounting_point_exemption_accounting_points_accounting_poin",
                        column: x => x.accounting_point_id,
                        principalTable: "accounting_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_point_exemption_exemption_categories_exemption_c",
                        column: x => x.exemption_category_id,
                        principalTable: "exemption_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_point_exemption_people_person_id",
                        column: x => x.person_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accounting_point_tariffs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accounting_point_id = table.Column<int>(nullable: false),
                    logs = table.Column<IReadOnlyCollection<ObjectLog>>(type: "jsonb", nullable: true),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    tariff_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounting_point_tariff", x => new { x.accounting_point_id, x.id });
                    table.ForeignKey(
                        name: "fk_accounting_point_tariff_accounting_points_accounting_point_id",
                        column: x => x.accounting_point_id,
                        principalTable: "accounting_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_point_tariff_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "tariffs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contracts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accounting_point_id = table.Column<int>(nullable: false),
                    logs = table.Column<IReadOnlyCollection<ObjectLog>>(type: "jsonb", nullable: true),
                    customer_id = table.Column<int>(nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: true),
                    send_paper_bill = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contract", x => new { x.accounting_point_id, x.id });
                    table.ForeignKey(
                        name: "fk_contract_accounting_points_accounting_point_id",
                        column: x => x.accounting_point_id,
                        principalTable: "accounting_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_contract_people_customer_id",
                        column: x => x.customer_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    period_id = table.Column<int>(nullable: false),
                    accounting_point_id = table.Column<int>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    from_date = table.Column<DateTime>(type: "date", nullable: false),
                    to_date = table.Column<DateTime>(type: "date", nullable: false),
                    usage_t1 = table.Column<Usage>(type: "jsonb", nullable: true),
                    usage_t2 = table.Column<Usage>(type: "jsonb", nullable: true),
                    usage_t3 = table.Column<Usage>(type: "jsonb", nullable: true),
                    total_units = table.Column<int>(nullable: false),
                    exemption_coeff = table.Column<decimal>(nullable: true),
                    total_discount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    total_amount_due = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    total_charge = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    incoming_balance = table.Column<decimal>(nullable: false),
                    counter_serial_number = table.Column<string>(nullable: true),
                    tariff_id = table.Column<int>(nullable: false),
                    note = table.Column<string>(nullable: true),
                    type = table.Column<int>(nullable: false),
                    zone_record = table.Column<int>(nullable: false),
                    dso_consumption_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoices", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoices_accounting_points_accounting_point_id",
                        column: x => x.accounting_point_id,
                        principalTable: "accounting_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invoices_periods_period_id",
                        column: x => x.period_id,
                        principalTable: "periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invoices_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "tariffs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    period_id = table.Column<int>(nullable: false),
                    batch_id = table.Column<int>(nullable: true),
                    accounting_point_id = table.Column<int>(nullable: true),
                    pay_date = table.Column<DateTime>(nullable: false),
                    enter_date = table.Column<DateTime>(nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    status = table.Column<int>(nullable: false),
                    payer_info = table.Column<string>(nullable: true),
                    accounting_point_name = table.Column<string>(nullable: true),
                    type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payments", x => x.id);
                    table.ForeignKey(
                        name: "fk_payments_accounting_points_accounting_point_id",
                        column: x => x.accounting_point_id,
                        principalTable: "accounting_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_payments_payment_batches_batch_id",
                        column: x => x.batch_id,
                        principalTable: "payment_batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_payments_periods_period_id",
                        column: x => x.period_id,
                        principalTable: "periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoice_payment_item",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    invoice_id = table.Column<int>(nullable: false),
                    payment_id = table.Column<int>(nullable: false),
                    amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_payment_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_payment_item_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invoice_payment_item_payments_payment_id",
                        column: x => x.payment_id,
                        principalTable: "payments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "building_types",
                columns: new[] { "id", "heataing_correction", "name" },
                values: new object[,]
                {
                    { 1, 1.0940m, "1-2 поверхи" },
                    { 3, 0.7980m, "3 поверхи і більше" }
                });

            migrationBuilder.InsertData(
                table: "distribution_system_operators",
                columns: new[] { "id", "commodity", "name" },
                values: new object[,]
                {
                    { 2, Commodity.ElectricPower, "АТ «Укрзалізниця»" },
                    { 1, Commodity.ElectricPower, "АТ «Житомиробленерго»" }
                });

            migrationBuilder.InsertData(
                table: "exemption_categories",
                columns: new[] { "id", "coeff", "effective_date", "end_date", "has_limit", "name" },
                values: new object[,]
                {
                    { 4, 100.0m, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Учасник бойових дій та членів родин загиблих в АТО (ООС)" },
                    { 3, 50.0m, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Iнвалiди 2 групи по зору або з ураженням ОРА" },
                    { 2, 50.0m, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Iнвалiди 1 групи по зору або з ураженням ОРА" },
                    { 1, 100.0m, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Почесні громадяни міста" }
                });

            migrationBuilder.InsertData(
                table: "periods",
                columns: new[] { "id", "end_date", "name", "start_date" },
                values: new object[,]
                {
                    { 202002, new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Лютий 2020р.", new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 202004, new DateTime(2020, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Квітень 2020р.", new DateTime(2020, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 202005, new DateTime(2020, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Травень 2020р.", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 202009, new DateTime(2020, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Вересень 2020р.", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 202007, new DateTime(2020, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Липень 2020р.", new DateTime(2020, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 202008, new DateTime(2020, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Серпень 2020р.", new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 202001, new DateTime(2020, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Січень 2020р.", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 202006, new DateTime(2020, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Червень 2020р.", new DateTime(2020, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 201912, new DateTime(2019, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Грудень 2019р.", new DateTime(2019, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 202003, new DateTime(2020, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Березень 2020р.", new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 201910, new DateTime(2019, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Жовтень 2019р.", new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 201909, new DateTime(2019, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Вересень 2019р.", new DateTime(2019, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 201908, new DateTime(2019, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Серпень 2019р.", new DateTime(2019, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 201907, new DateTime(2019, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Липень 2019р.", new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 201906, new DateTime(2019, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Червень 2019р.", new DateTime(2019, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 201905, new DateTime(2019, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Травень 2019р.", new DateTime(2019, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 201904, new DateTime(2019, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Квітень 2019р.", new DateTime(2019, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 201903, new DateTime(2019, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Березень 2019р.", new DateTime(2019, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 201902, new DateTime(2019, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Лютий 2019р.", new DateTime(2019, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 201911, new DateTime(2019, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Листопад 2019р.", new DateTime(2019, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 201901, new DateTime(2019, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Січень 2019р.", new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "regions",
                columns: new[] { "id", "name" },
                values: new object[] { 1, "Житомирська обл." });

            migrationBuilder.InsertData(
                table: "tariffs",
                columns: new[] { "id", "commodity", "name" },
                values: new object[,]
                {
                    { 2, Commodity.ElectricPower, "Будинки з електроопалювальними установками" },
                    { 1, Commodity.ElectricPower, "Населення (загальний тариф)" },
                    { 4, Commodity.ElectricPower, "Багатодітні, прийомні сім'ї та дитячі будинки сімейного типу" },
                    { 3, Commodity.ElectricPower, "Багатоквартирні негазифіковані будинки" }
                });

            migrationBuilder.InsertData(
                table: "usage_categories",
                columns: new[] { "id", "exemption_discount_norms", "name" },
                values: new object[,]
                {
                    { 4, null, "Електроопалювальна установка та електроплита" },
                    { 3, null, "Електроопалювальна установка" },
                    { 2, null, "Електроплита" },
                    { 1, null, "Звичайне споживання" }
                });

            migrationBuilder.InsertData(
                table: "zone_coeffs",
                columns: new[] { "id", "discount_weight", "start_date", "value", "zone_number", "zone_record" },
                values: new object[,]
                {
                    { 6, 0.21m, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1.5m, 3, 3 },
                    { 4, 0.46m, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.4m, 1, 3 },
                    { 2, 0.67m, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.5m, 1, 2 },
                    { 1, 1m, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1m, 1, 1 },
                    { 5, 0.33m, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1m, 2, 3 },
                    { 3, 0.33m, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1m, 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "branch_offices",
                columns: new[] { "id", "address", "available_commodities", "current_period_id", "district_ids", "name", "string_id" },
                values: new object[,]
                {
                    { 4, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 4 }, "Брусилівський ЦОК", "br" },
                    { 3, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 3 }, "Бердичiвський ЦОК", "bd" },
                    { 5, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 5 }, "Хорошівський ЦОК", "hr" },
                    { 6, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 6 }, "Ємільчинський ЦОК", "em" },
                    { 7, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 7 }, "Житомирський ЦОК", "zt" },
                    { 8, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 7 }, "Зарічанський ЦОК", "zr" },
                    { 9, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 8, 10 }, "Коростенський ЦОК", "kr" },
                    { 10, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 9 }, "Коростишiвський ЦОК", "kt" },
                    { 11, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 11 }, "Любарський ЦОК", "lb" },
                    { 12, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 12 }, "Малинський ЦОК", "ml" },
                    { 2, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 2 }, "Баранiвський ЦОК", "bn" },
                    { 13, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 13 }, "Народицький ЦОК", "nr" },
                    { 15, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 15 }, "Овруцький ЦОК", "ov" },
                    { 16, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 16 }, "Олевський ЦОК", "ol" },
                    { 17, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 17, 20 }, "Попільнянський ЦОК", "pp" },
                    { 18, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 18 }, "Радомишльський ЦОК", "rd" },
                    { 19, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 19 }, "Романівський ЦОК", "rm" },
                    { 20, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 21 }, "Пулинський ЦОК", "pl" },
                    { 21, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 22 }, "Черняхівський ЦОК", "ch" },
                    { 22, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 23 }, "Чуднівський ЦОК", "cd" },
                    { -1, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.NaturalGas }, 201901, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 }, "Центральний офіс", "co" },
                    { 14, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 14 }, "Новоград-Волинський ЦОК", "nv" },
                    { 1, "10003, м. Житомир, майдан Перемоги, 10", new[] { Commodity.ElectricPower }, 201901, new[] { 1 }, "Андрушівський ЦОК", "an" }
                });

            migrationBuilder.InsertData(
                table: "districts",
                columns: new[] { "id", "name", "region_id" },
                values: new object[,]
                {
                    { 1, "Андрушівський район", 1 },
                    { 22, "Черняхівський район", 1 },
                    { 2, "Баранiвський район", 1 },
                    { 3, "Бердичiвський район", 1 },
                    { 4, "Брусилівський район", 1 },
                    { 5, "Хорошівський район", 1 },
                    { 6, "Ємільчинський район", 1 },
                    { 7, "Житомирський район", 1 },
                    { 8, "Коростенський район", 1 },
                    { 23, "Чуднівський район", 1 },
                    { 10, "Лугинський район", 1 },
                    { 11, "Любарський район", 1 },
                    { 9, "Коростишiвський район", 1 },
                    { 13, "Народицький район", 1 },
                    { 14, "Новоград-Волинський район", 1 },
                    { 15, "Овруцький район", 1 },
                    { 16, "Олевський район", 1 },
                    { 17, "Попільнянський район", 1 },
                    { 18, "Радомишльський район", 1 },
                    { 19, "Романівський район", 1 },
                    { 20, "Ружинський район", 1 },
                    { 12, "Малинський район", 1 },
                    { 21, "Пулинський район", 1 }
                });

            migrationBuilder.InsertData(
                table: "tariff_rates",
                columns: new[] { "id", "consumption_limit", "heating_consumption_limit", "heating_end_day", "heating_start_day", "start_date", "tariff_id", "value" },
                values: new object[,]
                {
                    { 5, 100, 3000, new DateTime(2020, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2017, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 0.90m },
                    { 4, null, null, null, null, new DateTime(2017, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1.68m },
                    { 6, null, null, null, null, new DateTime(2017, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1.68m },
                    { 2, null, null, null, null, new DateTime(2017, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1.68m },
                    { 1, 100, null, null, null, new DateTime(2017, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0.9m },
                    { 3, 100, 3000, new DateTime(2020, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2017, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 0.90m },
                    { 7, null, null, null, null, new DateTime(2017, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 0.90m }
                });

            migrationBuilder.CreateIndex(
                name: "ix_accounting_point_exemption_exemption_category_id",
                table: "accounting_point_exemptions",
                column: "exemption_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_point_exemption_person_id",
                table: "accounting_point_exemptions",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_point_tariff_tariff_id",
                table: "accounting_point_tariffs",
                column: "tariff_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_address_id",
                table: "accounting_points",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_branch_office_id",
                table: "accounting_points",
                column: "branch_office_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_building_type_id",
                table: "accounting_points",
                column: "building_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_distribution_system_operator_id",
                table: "accounting_points",
                column: "distribution_system_operator_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_eic",
                table: "accounting_points",
                column: "eic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_name",
                table: "accounting_points",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_owner_id",
                table: "accounting_points",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_usage_category_id",
                table: "accounting_points",
                column: "usage_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_addresses_street_id",
                table: "addresses",
                column: "street_id");

            migrationBuilder.CreateIndex(
                name: "ix_branch_offices_current_period_id",
                table: "branch_offices",
                column: "current_period_id");

            migrationBuilder.CreateIndex(
                name: "ix_cities_district_id",
                table: "cities",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "ix_contract_customer_id",
                table: "contracts",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_districts_region_id",
                table: "districts",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_payment_item_invoice_id",
                table: "invoice_payment_item",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_payment_item_payment_id",
                table: "invoice_payment_item",
                column: "payment_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_accounting_point_id",
                table: "invoices",
                column: "accounting_point_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_dso_consumption_id",
                table: "invoices",
                column: "dso_consumption_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_invoices_period_id",
                table: "invoices",
                column: "period_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_tariff_id",
                table: "invoices",
                column: "tariff_id");

            migrationBuilder.CreateIndex(
                name: "ix_payment_batches_branch_office_id",
                table: "payment_batches",
                column: "branch_office_id");

            migrationBuilder.CreateIndex(
                name: "ix_payment_batches_payment_channel_id",
                table: "payment_batches",
                column: "payment_channel_id");

            migrationBuilder.CreateIndex(
                name: "ix_payments_accounting_point_id",
                table: "payments",
                column: "accounting_point_id");

            migrationBuilder.CreateIndex(
                name: "ix_payments_batch_id",
                table: "payments",
                column: "batch_id");

            migrationBuilder.CreateIndex(
                name: "ix_payments_period_id",
                table: "payments",
                column: "period_id");

            migrationBuilder.CreateIndex(
                name: "ix_people_address_id",
                table: "people",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "ix_people_id_card_number",
                table: "people",
                column: "id_card_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_people_tax_code",
                table: "people",
                column: "tax_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_periods_start_date",
                table: "periods",
                column: "start_date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_streets_city_id",
                table: "streets",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_tariff_rate_tariff_id",
                table: "tariff_rates",
                column: "tariff_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounting_point_debt_history");

            migrationBuilder.DropTable(
                name: "accounting_point_exemptions");

            migrationBuilder.DropTable(
                name: "accounting_point_tariffs");

            migrationBuilder.DropTable(
                name: "contracts");

            migrationBuilder.DropTable(
                name: "invoice_payment_item");

            migrationBuilder.DropTable(
                name: "tariff_rates");

            migrationBuilder.DropTable(
                name: "zone_coeffs");

            migrationBuilder.DropTable(
                name: "exemption_categories");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "tariffs");

            migrationBuilder.DropTable(
                name: "accounting_points");

            migrationBuilder.DropTable(
                name: "payment_batches");

            migrationBuilder.DropTable(
                name: "building_types");

            migrationBuilder.DropTable(
                name: "distribution_system_operators");

            migrationBuilder.DropTable(
                name: "people");

            migrationBuilder.DropTable(
                name: "usage_categories");

            migrationBuilder.DropTable(
                name: "branch_offices");

            migrationBuilder.DropTable(
                name: "payment_channels");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "periods");

            migrationBuilder.DropTable(
                name: "streets");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "districts");

            migrationBuilder.DropTable(
                name: "regions");
        }
    }
}
