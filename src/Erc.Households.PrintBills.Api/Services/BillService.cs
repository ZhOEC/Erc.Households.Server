using Dapper;
using DocumentFormat.OpenXml.ReportBuilder;
using Erc.Households.PrintBills.Api.Models;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.PrintBills.Api.Services
{
    public class BillService
    {
        IDbConnection _dbConnection;

        public BillService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Stream> GetNaturalGasBillsByPeriodAsync(int periodId)
        {
            return await GetNaturalGasBillsInternalAsync(new SqlParams { PeriodId = periodId });
        }

        public async Task<Stream> GetNaturalGasBill(int id)
        {
            return await GetNaturalGasBillsInternalAsync(new SqlParams { Id = id });
        }

        private async Task<Stream> GetNaturalGasBillsInternalAsync(SqlParams @params)
        {
            var bills =  await _dbConnection.QueryAsync<Bill>(@$"select
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
    join accounting_point_debt_history as apdh on ap.id = apdh.accounting_point_id and apdh.period_id=inv.period_id
    left join LATERAL
        (
            select p.accounting_point_id, sum(p.amount) as sumAmount
            from payments as p
            where period_id = inv.period_id and status = 1 and p.accounting_point_id=inv.accounting_point_id
            group by p.accounting_point_id
        )  payments on true
where ap.branch_office_id = 101 and inv.period_id = case when @periodId is null then inv.period_id else @periodId end
and inv.id = case when @id is null then inv.id else @id end", @params);
            
            var dict = new Dictionary<string, IList>
            {
                { "bill_gas", bills.ToList() }
            };

            var ms = ReportBuilderXLS.GenerateReport(dict, "Templates/bill_gas.xlsx");
            ms.Position = 0;
            return ms;
        }

        class SqlParams
        {
            public int? Id { get; set; }
            public int? PeriodId { get; set; }
        }
    }
}
