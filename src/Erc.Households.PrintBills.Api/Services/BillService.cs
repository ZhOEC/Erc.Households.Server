using ClosedXML.Excel;
using ClosedXML.Report;
using Dapper;
using DocumentFormat.OpenXml.ReportBuilder;
using Erc.Households.Domain.Shared;
using Erc.Households.PrintBills.Api.Models;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Erc.Households.PrintBills.Api.Services
{
    public class BillService
    {
        readonly IDbConnection _dbConnection;
        readonly IEnumerable<RecsBillLocation> _recsBillLocations;
        readonly HttpClient _httpClient;

        public BillService(IDbConnection dbConnection, IOptions<List<RecsBillLocation>> options, HttpClient httpClient)
        {
            _dbConnection = dbConnection;
            _recsBillLocations = options.Value;
            _httpClient = httpClient;
        }

        public async Task<Stream> GetBillById(Commodity commodity, int billId)
        {
            return commodity switch
            {
                Commodity.ElectricPower => await GetElectricityBillsInternalAsync(new SqlParams { Id = billId }),
                Commodity.NaturalGas => await GetNaturalGasBillsInternalAsync(new SqlParams { Id = billId }),
                _ => null,
            };
        }

        public async Task<Stream> GetBillsByPeriod(FileType fileType, int branchOfficeId, Commodity commodity, int periodId)
        {
            return commodity switch
            {
                 Commodity.ElectricPower => await GetElectricityBillsInternalAsync(new SqlParams { BranchOfficeId = branchOfficeId, PeriodId = periodId}, fileType),
                 Commodity.NaturalGas => await GetNaturalGasBillsInternalAsync(new SqlParams { PeriodId = periodId }, fileType),
                 _ => null
            };
        }

        private async Task<Stream> GetNaturalGasBillsInternalAsync(SqlParams @params, FileType fileType = FileType.Excel)
        {
            var bills = await _dbConnection.QueryAsync<BillNaturalGas>(@$"
                select
                   company.name as CompanyFullName,
                   company.short_name as CompanyShortName,
                   company.www as CompanySite,
                   company.email as CompanyEmail,
                   company.taxpayer_phone as CompanyPhone,
                   company.state_registry_code as CompanyStateRegistryCode,
                   company.address as CompanyAddress,
                   branch_offices.iban as BranchOfficeIban,
                   branch_offices.bank_full_name as BranchOfficeBankFullName,
                   ap.name as AccountingPointName,
                   concat(address.cityName, ' ', address.streetName, ' ', address.building, ' ', address.apt) as AccountingPointAddress,
                   concat(people.last_name, ' ', people.first_name, ' ', people.patronymic) as OwnerFullName,
                   periods.name as PeriodShortDate,
                   apdh.debt_value::decimal as AccountingPointDebtHistory,
                   payments.sumAmount::decimal as PaymentSumByPeriod,
                   inv.total_units::decimal as InvoiceTotalUnits,
                   inv.total_amount_due::decimal as InvoiceTotalAmountDue,
                   (jsonb_array_elements(inv.usage_t1 -> 'Calculations') -> 'PriceValue')::decimal as TariffRate,
                   ap.debt::decimal as AccountingPointDebt
                from invoices inv
                    join accounting_points ap on ap.id = inv.accounting_point_id
                    join branch_offices on ap.branch_office_id = branch_offices.id
                        join company on branch_offices.company_id = company.id
                        join periods on branch_offices.current_period_id = periods.id
                    join
                        (
                            select addresses.id, cities.name as cityName, streets.name as streetName, building, apt
                            from addresses
                            join streets on addresses.street_id = streets.id
                            join cities on streets.city_id = cities.id
                        ) as address on ap.address_id = address.id
                    join people on ap.owner_id = people.id
                    left join accounting_point_debt_history as apdh on ap.id = apdh.accounting_point_id and apdh.period_id=inv.period_id
                    left join LATERAL
                        (
                            select p.accounting_point_id, sum(p.amount) as sumAmount
                            from payments as p
                            where period_id = inv.period_id and status = 1 and p.accounting_point_id=inv.accounting_point_id
                            group by p.accounting_point_id
                        )  payments on true
                where ap.branch_office_id = case when @branchOfficeId is null then 101 else @branchOfficeId end
                and inv.period_id = case when @periodId is null then inv.period_id else @periodId end
                and inv.id = case when @id is null then inv.id else @id end", @params);

            var ms = new MemoryStream();
            if (fileType == FileType.Csv) ms = CsvHandler(bills);
            else if (fileType == FileType.Excel) ms = XlsxHandler(bills);

            return ms;
        }

        private async Task<Stream> GetElectricityBillsInternalAsync(SqlParams @params, FileType fileType = FileType.Excel)
        {
            var bills = await _dbConnection.QueryAsync<BillElectricity>(@$"
                select
                    ap.id as AccountinPointId
                   ,company.name as CompanyFullName
                   ,company.short_name as CompanyShortName
                   ,company.www as CompanySite
                   ,company.email as CompanyEmail
                   ,company.state_registry_code as CompanyStateRegistryCode
                   ,company.address as CompanyAddress
                   ,branch_offices.name as BranchOfficeName
                   ,branch_offices.address as BranchOfficeAddress
                   ,branch_offices.iban as BranchOfficeIban
                   ,branch_offices.bank_full_name as BranchOfficeBankFullName
                   ,ap.name as AccountingPointName
                   ,address.zip as Zip
                   ,address.cityName as City
                   ,ap.Eic as Eic
                   ,concat(address.cityName, ' ', address.streetName, ' ', address.building, ' ', address.apt) as AccountingPointAddress
                   ,concat(people.last_name, ' ', people.first_name, ' ', people.patronymic) as OwnerFullName
                   ,periods.name as PeriodName
                   ,apdh.debt_value as AccountingPointDebtHistory
                   ,payments.sumAmount as PaymentsSumByPeriod
                   ,payments_compensation.SumAmount AS CompensationSumByPeriod
                   ,inv.total_units as InvoiceTotalUnits
                   ,inv.total_amount_due as InvoiceTotalAmountDue
                   ,(inv.usage_t1 -> 'DiscountUnits')::numeric + (inv.usage_t2 -> 'DiscountUnits')::numeric + (inv.usage_t3 -> 'DiscountUnits')::numeric as DiscountUnitsSum
                   ,(inv.usage_t1 -> 'Discount')::decimal + (inv.usage_t2 -> 'Discount')::decimal + (inv.usage_t3 -> 'Discount')::decimal as DiscountSum
                   ,ap.debt as AccountingPointDebt
                   ,to_char(cntr.start_date, 'dd.MM.yyyy') as ContractStartDate
                   ,inv.usage_t1 as UsageT1
                   ,inv.usage_t2 as UsageT2
                   ,inv.usage_t3 as UsageT3
                from invoices inv
                    join accounting_points ap on ap.id = inv.accounting_point_id
                    join branch_offices on ap.branch_office_id = branch_offices.id
                    join company on branch_offices.company_id = company.id
                    join periods on inv.period_id = periods.id
                    join
                        (
                            select addresses.id, cities.name as cityName, streets.name as streetName, building, apt, zip
                            from addresses
                            join streets ON addresses.street_id = streets.id
                            join cities ON streets.city_id = cities.id
                        ) as address ON ap.address_id = address.id
                    JOIN people ON ap.owner_id = people.id
                    LEFT JOIN accounting_point_debt_history as apdh on ap.id = apdh.accounting_point_id and apdh.period_id = inv.period_id
                    LEFT JOIN LATERAL
                    (
                      select p.accounting_point_id, SUM(p.amount) AS SumAmount
                      from payments as p
                      where period_id = inv.period_id AND status = 1 and p.accounting_point_id = inv.accounting_point_id AND p.type <> 2
                      group by p.accounting_point_id
                    ) payments ON TRUE
                    LEFT JOIN LATERAL
                    (
                      select SUM(p.amount) AS SumAmount
                      from payments as p
                      where period_id = inv.period_id AND status = 1 and p.accounting_point_id = inv.accounting_point_id AND p.type = 2
                      group by p.accounting_point_id
                    ) payments_compensation ON TRUE
                    left join contracts cntr on ap.id = cntr.accounting_point_id
                where ap.branch_office_id = case when @branchOfficeId is null then ap.branch_office_id else @branchOfficeId end
                    and inv.period_id = case when @periodId is null then inv.period_id else @periodId end
                    and inv.id = case when @id is null then inv.id else @id end         
                    limit 100", @params);

            var ms = new MemoryStream();
            if (fileType == FileType.Csv) ms = CsvHandler(bills);
            else if (fileType == FileType.Excel) ms = XlsxHandler(bills);

            return ms;
        }

        private MemoryStream XlsxHandler(IEnumerable<object> bills)
        {
            var ms = new MemoryStream();
            if (bills is IEnumerable<BillElectricity>)
            {
                var template = new XLTemplate("Templates/bill_electricity.xlsx");

                var billsT1 = bills.Cast<BillElectricity>().Where(x => x.UsageT2 is null && x.UsageT3 is null).ToList();
                var billsT2 = bills.Cast<BillElectricity>().Where(x => x.UsageT2 != null && x.UsageT3 is null).ToList();
                var billsT3 = bills.Cast<BillElectricity>().Where(x => x.UsageT2 != null && x.UsageT3 != null).ToList();

                if (billsT3.Count() == 0)
                    template.Workbook.Worksheet(3).Delete();
                if (billsT2.Count() == 0)
                    template.Workbook.Worksheet(2).Delete();
                if (billsT1.Count() == 0)
                    template.Workbook.Worksheet(1).Delete();

                template.AddVariable("bill_t1", billsT1);
                template.AddVariable("bill_t2", billsT2);
                template.AddVariable("bill_t3", billsT3);

                template.Generate();
                template.SaveAs(ms);
            } 
            else if (bills is IEnumerable<BillNaturalGas>)
            {
                var dict = new Dictionary<string, IList>
                {
                    { "bill_gas", bills.ToList() }
                };

                ms = ReportBuilderXLS.GenerateReport(dict, "Templates/bill_gas.xlsx");
                var counter = 1;
                using var naturalGasBills = new XLWorkbook(ms);
                foreach (var bill in bills)
                {
                    var ws = naturalGasBills.Worksheet(1);
                    ws.AddPicture(@"Templates/n_gas.png")
                            .MoveTo(ws.Cell($"A{(counter - 1) * 19 + 7}"))
                               .Scale(0.66); // optional: resize picture
                    if (counter != 0 && counter % 4 == 0)
                        ws.PageSetup.AddHorizontalPageBreak(counter * 19);
                    counter++;
                }
                naturalGasBills.SaveAs(ms);
            }

            ms.Position = 0;
            return ms;
        }

        private MemoryStream CsvHandler(IEnumerable<object> bills)
        {
            var ms = new MemoryStream();
            var wr = new StreamWriter(ms, Encoding.GetEncoding(1251));

            if (bills is IEnumerable<BillElectricity>)
            {
                bills.Cast<BillElectricity>()
                    .ToList()
                    .ForEach(x =>
                    {
                        wr.WriteLine(
                            $"\"{x.AccountingPointName}\";" +
                            $"\"{x.PeriodName}\";" +
                            $"\"{x.ZoneCount}\";" +
                            $"\"{x.OwnerFullName}\";" +
                            $"\"{x.AccountingPointAddress}\";" +
                            $"\"{x.Zip}\";" +
                            $"\"{x.InvoiceTotalAmountDue}\";" +
                            $"\"{x.PaymentsSumByPeriod}\";" +
                            $"\"{x.UsageT1.PresentMeterReading}\";" +
                            $"\"{x.UsageT1.PreviousMeterReading}\";" +
                            $"\"{x.UsageT1.PresentMeterReading}\";" +
                            $"\"{x.UsageT1.PreviousMeterReading}\";" +
                            $"\"{x.UsageT2?.PresentMeterReading}\";" +
                            $"\"{x.UsageT2?.PreviousMeterReading}\";" +
                            $"\"{x.UsageT3?.PresentMeterReading}\";" +
                            $"\"{x.UsageT3?.PreviousMeterReading}\";" +
                            $"\"{x.UsageT1.Kz}\";" +
                            $"\"{x.UsageT1.Units}\";" +
                            $"\"{x.UsageT1.Charge}\";" +
                            $"\"{x.UsageT1.Discount}\";" +
                            $"\"{x.UsageT1.DiscountUnits}\";" +
                            $"\"{x.UsageT2?.Kz}\";" +
                            $"\"{x.UsageT2?.Units}\";" +
                            $"\"{x.UsageT2?.Charge}\";" +
                            $"\"{x.UsageT2?.Discount}\";" +
                            $"\"{x.UsageT2?.DiscountUnits}\";" +
                            $"\"{x.UsageT3?.Kz}\";" +
                            $"\"{x.UsageT3?.Units}\";" +
                            $"\"{x.UsageT3?.Charge}\";" +
                            $"\"{x.UsageT3?.Discount}\";" +
                            $"\"{x.UsageT3?.DiscountUnits}\";" +
                            //$"\"{}\";" + // % discount
                            $"\"{x.City}\";" +
                            $"\"{x.InvoiceTotalAmountDue}\";" +
                            $"\"{x.DiscountUnitsSum}\";" +
                            $"\"{x.DiscountSum}\";" +
                            //$"\"{}\";" + // qr code
                            $"\"{x.InvoiceTotalUnits}\";" +
                            $"\"{x.Eic}\";" +
                            $"\"{x.CompanyFullName}\";" +
                            $"\"{x.CompanyStateRegistryCode}\";" +
                            $"\"{x.BranchOfficeName}\";" +
                            $"\"{x.BranchOfficeAddress}\";" +
                            $"\"{x.BranchOfficeBankFullName}\";" +
                            //$"\"{}\";" + // mfo
                            $"\"{x.BranchOfficeIban}\";" +
                            //$"\"{}\";" + //compensation
                            $"\"{x.ContractStartDate}\";" +
                            $"\"{x.AccountingPointDebtHistory}\";" +
                            $"\"{x.PaymentsSumByPeriod}\";" +
                            $"\"{x.Barcode}\";" +
                            $"\"{x.UsageT1.GroupCalculations.ElementAtOrDefault(0)?.PriceValue}\";" +
                            $"\"{x.UsageT1.GroupCalculations.ElementAtOrDefault(0)?.Units}\";" +
                            $"\"{x.UsageT1.GroupCalculations.ElementAtOrDefault(0)?.Charge}\";" +
                            $"\"{x.UsageT1.GroupCalculations.ElementAtOrDefault(0)?.DiscountUnits}\";" +
                            $"\"{x.UsageT1.GroupCalculations.ElementAtOrDefault(0)?.Discount}\";" +
                            $"\"{x.UsageT1.GroupCalculations.ElementAtOrDefault(1)?.PriceValue}\";" +
                            $"\"{x.UsageT1.GroupCalculations.ElementAtOrDefault(1)?.Units}\";" +
                            $"\"{x.UsageT1.GroupCalculations.ElementAtOrDefault(1)?.Charge}\";" +
                            $"\"{x.UsageT1.GroupCalculations.ElementAtOrDefault(1)?.DiscountUnits}\";" +
                            $"\"{x.UsageT1.GroupCalculations.ElementAtOrDefault(1)?.Discount}\";" +

                            $"\"{x.UsageT2?.GroupCalculations.ElementAtOrDefault(0).PriceValue}\";" +
                            $"\"{x.UsageT2?.GroupCalculations.ElementAtOrDefault(0).Units}\";" +
                            $"\"{x.UsageT2?.GroupCalculations.ElementAtOrDefault(0).Charge}\";" +
                            $"\"{x.UsageT2?.GroupCalculations.ElementAtOrDefault(0).DiscountUnits}\";" +
                            $"\"{x.UsageT2?.GroupCalculations.ElementAtOrDefault(0).Discount}\";" +
                            $"\"{x.UsageT2?.GroupCalculations.ElementAtOrDefault(1)?.PriceValue}\";" +
                            $"\"{x.UsageT2?.GroupCalculations.ElementAtOrDefault(1)?.Units}\";" +
                            $"\"{x.UsageT2?.GroupCalculations.ElementAtOrDefault(1)?.Charge}\";" +
                            $"\"{x.UsageT2?.GroupCalculations.ElementAtOrDefault(1)?.DiscountUnits}\";" +
                            $"\"{x.UsageT2?.GroupCalculations.ElementAtOrDefault(1)?.Discount}\";" +

                            $"\"{x.UsageT3?.GroupCalculations.ElementAtOrDefault(0).PriceValue}\";" +
                            $"\"{x.UsageT3?.GroupCalculations.ElementAtOrDefault(0).Units}\";" +
                            $"\"{x.UsageT3?.GroupCalculations.ElementAtOrDefault(0).Charge}\";" +
                            $"\"{x.UsageT3?.GroupCalculations.ElementAtOrDefault(0).DiscountUnits}\";" +
                            $"\"{x.UsageT3?.GroupCalculations.ElementAtOrDefault(0).Discount}\";" +
                            $"\"{x.UsageT3?.GroupCalculations.ElementAtOrDefault(1)?.PriceValue}\";" +
                            $"\"{x.UsageT3?.GroupCalculations.ElementAtOrDefault(1)?.Units}\";" +
                            $"\"{x.UsageT3?.GroupCalculations.ElementAtOrDefault(1)?.Charge}\";" +
                            $"\"{x.UsageT3?.GroupCalculations.ElementAtOrDefault(1)?.DiscountUnits}\";" +
                            $"\"{x.UsageT3?.GroupCalculations.ElementAtOrDefault(1)?.Discount}\";" +
                            $"");
                    });
            }
            else if (bills is IEnumerable<BillNaturalGas>)
            {
                // TODO: Add implementation if need
            }

            wr.Flush();
            ms.Position = 0;
            return ms;
        }

        class SqlParams
        {
            public int? Id { get; set; }
            public int? BranchOfficeId { get; set; }
            public int? PeriodId { get; set; }
        }
        
    }
}
