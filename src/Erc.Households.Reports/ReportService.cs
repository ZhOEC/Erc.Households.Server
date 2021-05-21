using ClosedXML.Excel;
using ClosedXML.Report;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Reports
{
    public class ReportService
    {
        IDbConnection _dbConnection;

        public ReportService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Stream> CreateReportAsync(string slug, int branchOfficeId, int periodId, int[] dsoIds)
        {
            return slug switch
            {
                "tobs" => await CreateTurnoverBalanceSheetAsync(branchOfficeId, periodId, dsoIds),
                "tobspl" => await CreateTurnoverBalanceSheetPersonListAsync(branchOfficeId, periodId, dsoIds),
                _ => throw new ArgumentException("Undefined report slug", nameof(slug)),
            };
        }

        private async Task<Stream> CreateTurnoverBalanceSheetAsync(int branchOfficeId, int periodId, int[] dsoIds)
        {
            var data = await _dbConnection.QueryFirstAsync(@"
    select
       per.name as ""PeriodName"",
       (select name from branch_offices where id=@branchOfficeId) as ""BranchOfficeName"",
       coalesce(accounting_point_debt_history.StartDebitSum, 0) as ""StartDebitSum"",
       coalesce(accounting_point_debt_history.TaxStartDebitSum, 0) as ""TaxStartDebitSum"",
       coalesce(accounting_point_debt_history.StartCreditSum, 0) as ""StartCreditSum"",
       coalesce(accounting_point_debt_history.TaxStartCreditSum, 0) as ""TaxStartCreditSum"",
       coalesce(accounting_point_debt_history.StartBalanceSum, 0) as ""StartBalanceSum"",
       coalesce(accounting_point_debt_history.TaxStartBalanceSum, 0) as ""TaxStartBalanceSum"",

       coalesce(start_debit.UsedUnitsSum, 0) as ""UsedUnitsSum"",
       coalesce(start_debit.UsedAmountDueSum, 0) as ""UsedAmountDueSum"",
       coalesce(start_debit.RecalculationAmountDuePlus, 0) as ""RecalculationAmountDuePlus"",
       start_debit.TaxRecalculationAmountDuePlus as ""TaxRecalculationAmountDuePlus"",
       coalesce(start_debit.RecalculationAmountDueMinus, 0) as ""RecalculationAmountDueMinus"",
       coalesce(start_debit.TaxRecalculationAmountDueMinus, 0) as ""TaxRecalculationAmountDueMinus"",
       coalesce(start_debit.TotalAmountDueSum, 0) as ""TotalAmountDueSum"",
       coalesce(start_debit.TaxTotalAmountDueSum, 0) as ""TaxTotalAmountDueSum"",
       
       coalesce(payments.PaymentAmountSum, 0) as ""PaymentAmountSum"",
       coalesce(payments.TaxPaymentAmountSum, 0) as ""TaxPaymentAmountSum"",
       coalesce(payments.PaymentCurrentPeriodSum, 0) as ""PaymentCurrentPeriodSum"",
       coalesce(payments.TaxPaymentCurrentPeriodSum, 0) as ""TaxPaymentCurrentPeriodSum"",
       coalesce(payments.PaymentNotCurrentPeriodPlusSum, 0) as ""PaymentNotCurrentPeriodPlusSum"",
       coalesce(payments.TaxPaymentNotCurrentPeriodPlusSum, 0) as ""TaxPaymentNotCurrentPeriodPlusSum"",
       coalesce(payments.PaymentNotCurrentPeriodMinusSum, 0) as ""PaymentNotCurrentPeriodMinusSum"",
       coalesce(payments.TaxPaymentNotCurrentPeriodMinusSum, 0) as ""TaxPaymentNotCurrentPeriodMinusSum"",
       coalesce(end_debit.EndDebitSum, 0) as ""EndDebitSum"",
       coalesce(end_debit.EndDebitSum, 0) / 6 as ""TaxEndDebitSum"",
       coalesce(end_debit.EndCreditSum, 0) as ""EndCreditSum"",
       coalesce(end_debit.EndCreditSum, 0) / 6 as ""TaxEndCreditSum"",
       coalesce(end_debit.EndBalanceSum, 0) as ""EndBalanceSum"",
       coalesce(end_debit.EndBalanceSum, 0) / 6 as ""TaxEndBalanceSum""
from periods per
left join
    (
        select
               apdh.period_id,
               sum(apdh.debt_value) StartBalanceSum,
               sum(apdh.debt_value) / 6 TaxStartBalanceSum,
               sum(case when apdh.debt_value > 0 then apdh.debt_value else 0 end) StartDebitSum,
               sum(case when apdh.debt_value > 0 then apdh.debt_value else 0 end) / 6 TaxStartDebitSum,
               sum(case when apdh.debt_value < 0 then apdh.debt_value else 0 end)  StartCreditSum, 
               sum(case when apdh.debt_value < 0 then apdh.debt_value else 0 end) / 6 TaxStartCreditSum
        from accounting_point_debt_history apdh
            join accounting_points ap on apdh.accounting_point_id = ap.id
        where ap.branch_office_id = @branchOfficeId and ap.distribution_system_operator_id = any (@dsoIds)
        group by apdh.period_id
    ) accounting_point_debt_history on per.id = accounting_point_debt_history.period_id
left join
    (
        select
               inv.period_id,
               sum(case when inv.type = 1 then inv.total_units else 0 end) UsedUnitsSum,
               sum(case when inv.type = 1 then inv.total_amount_due else 0 end) UsedAmountDueSum,
               sum(case when inv.total_amount_due > 0 and inv.type = 2 then inv.total_amount_due else 0 end) RecalculationAmountDuePlus,
               sum(case when inv.total_amount_due > 0 and inv.type = 2 then inv.total_amount_due else 0 end) / 6 TaxRecalculationAmountDuePlus,
               sum(case when inv.total_amount_due < 0 and inv.type = 2 then 0-inv.total_amount_due else 0 end) RecalculationAmountDueMinus,
               sum(case when inv.total_amount_due < 0 and inv.type = 2 then 0-inv.total_amount_due else 0 end) / 6 TaxRecalculationAmountDueMinus,
               sum(inv.total_amount_due) TotalAmountDueSum,
               sum(inv.total_amount_due) / 6 TaxTotalAmountDueSum
        from invoices inv
            join accounting_points ap on inv.accounting_point_id = ap.id
        where ap.branch_office_id = @branchOfficeId and ap.distribution_system_operator_id = any (@dsoIds)
        group by inv.period_id
        ) start_debit on start_debit.period_id = per.id
left join
    (
        select
               pay.period_id,
               sum(pay.amount) PaymentAmountSum,
               sum(pay.amount) / 6 TaxPaymentAmountSum,
               sum(case when pay.type <> 3 then pay.amount else 0 end) PaymentCurrentPeriodSum,
               sum(case when pay.type <> 3 then pay.amount else 0 end) / 6 TaxPaymentCurrentPeriodSum,
               sum(case when pay.amount > 0 and pay.type = 3 then pay.amount else 0 end) PaymentNotCurrentPeriodPlusSum,
               sum(case when pay.amount > 0 and pay.type = 3 then pay.amount else 0 end) / 6 TaxPaymentNotCurrentPeriodPlusSum,
               sum(case when pay.amount < 0 and pay.type = 3 then pay.amount else 0 end) PaymentNotCurrentPeriodMinusSum,
               sum(case when pay.amount < 0 and pay.type = 3 then pay.amount else 0 end) / 6 TaxPaymentNotCurrentPeriodMinusSum
        from payments pay
            join accounting_points ap on pay.accounting_point_id = ap.id
        where branch_office_id = @branchOfficeId and ap.distribution_system_operator_id = any (@dsoIds)
        group by pay.period_id
    ) payments on per.id = payments.period_id
left join lateral
    (
        select
               
               sum(case when bo.current_period_id > @periodId then apdh.debt_value else ap.debt end) EndBalanceSum,
               sum(case when bo.current_period_id > @periodId then case when apdh.debt_value > 0 then apdh.debt_value end else case when ap.debt > 0 then ap.debt end end) EndDebitSum,
               sum(case when bo.current_period_id > @periodId then case when apdh.debt_value < 0 then 0-apdh.debt_value end else case when ap.debt < 0 then 0-ap.debt end end) EndCreditSum
        from accounting_points ap
            left join accounting_point_debt_history apdh on ap.id = apdh.accounting_point_id and apdh.period_id=(select min(id) from periods where id>@periodId)
            join branch_offices bo on ap.branch_office_id = bo.id
        where bo.id = @branchOfficeId and ap.distribution_system_operator_id = any (@dsoIds)
    ) end_debit on true
 where per.id = @periodId", new { branchOfficeId, periodId, dsoIds });

            var report = new XLTemplate(@"Templates/turnover_balance_sheet.xlsx");
            foreach (var obj in data as IDictionary<string, object>)
                report.AddVariable(obj.Key, obj.Value);
            report.Generate();
            var ms = new MemoryStream();
            report.SaveAs(ms, new SaveOptions { EvaluateFormulasBeforeSaving = true });
            ms.Position = 0;
            return ms;
        }

        private async Task<Stream> CreateTurnoverBalanceSheetPersonListAsync(int branchOfficeId, int periodId, int[] dsoIds)
        {
            var data = await _dbConnection.QueryAsync<Person>(@"select bo.name bo_name, (select name from periods where id=@periodId) period_name
                , ap.eic, ap.name, last_name||' '||first_name||' '||patronymic person, to_char(c.start_date,'DD.MM.YYYY') start_date
                , start_debt.debt_value::decimal(10,2) start_debt, i.total_units::decimal(10,2), i.total_charge::decimal(10,2), payments.payed::decimal(10,2)
                , case when end_debt.period_id is null then ap.debt::decimal(10,2) else end_debt.debt_value::decimal(10,2) end end_debt
                from accounting_points ap 
                join people p on ap.owner_id=p.id
                join contracts c on ap.id=c.accounting_point_id and c.end_date is null
                left join (select sum(total_units) total_units, sum(total_charge) total_charge, accounting_point_id from invoices where period_id=@periodId group by accounting_point_id) i on ap.id=i.accounting_point_id
                left join (select sum(amount) payed, accounting_point_id from payments where period_id=@periodId group by accounting_point_id) payments on payments.accounting_point_id=ap.id
                left join accounting_point_debt_history start_debt on ap.id=start_debt.accounting_point_id and start_debt.period_id=@periodId
                left join accounting_point_debt_history end_debt on ap.id=end_debt.accounting_point_id and end_debt.period_id=(select min(id) from periods where id>@periodId)
                join branch_offices bo on ap.branch_office_id=bo.id
                where ap.branch_office_id = @branchOfficeId and ap.distribution_system_operator_id = any (@dsoIds)", new { branchOfficeId, periodId, dsoIds });

            var report = new XLTemplate(@"Templates/turnover_balance_sheet_people.xlsx");

            report.AddVariable(new
            {
                data.FirstOrDefault()?.period_name,
                data.FirstOrDefault()?.bo_name,
                People = data
            });
            report.Generate();
            var ms = new MemoryStream();
            report.SaveAs(ms);
            ms.Position = 0;
            return ms;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        class Person
        {
            public string bo_name { get; set; }
            public string period_name { get; set; }
            public string eic { get; set; }
            public string name { get; set; }
            public string person { get; set; }
            public string start_date { get; set; }
            public decimal start_debt { get; set; }
            public decimal total_units { get; set; }
            public decimal total_charge { get; set; }
            public decimal payed { get; set; }
            public decimal end_debt { get; set; }
        }
    }
}
