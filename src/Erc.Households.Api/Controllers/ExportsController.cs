using Erc.Households.EF.PostgreSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportsController : ControllerBase
    {
        private readonly ErcContext _ercContext;

        public ExportsController(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        [HttpGet("recordpoints")]
        public async Task<IActionResult> GetRecordpoints(string type, DateTime date)
        {
            var ms = new MemoryStream();
            var wr = new StreamWriter(ms, Encoding.GetEncoding(1251));
            using var con = _ercContext.Database.GetDbConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = $@"select 
                        ap.name, COALESCE(p.tax_code, p.id_card_number), to_char(c.start_date,'DD.MM.YYYY'), a.zip, cities.name, s.name, a.building, coalesce(a.apt, ''), tv.value
                        from accounting_points ap
                        join people p on p.id=ap.owner_id
                        join contracts c on ap.id=c.accounting_point_id and c.end_date is null
                        join addresses a on ap.address_id=a.id
                        join streets s on a.street_id=s.id 
                        join cities on city_id=cities.id
                        join accounting_point_tariffs apt on apt.accounting_point_id = ap.id 
                        join (
                        select id, rates.""Value"" as value, rates.""StartDate"" as start_date from tariffs, jsonb_to_recordset(tariffs.rates) as rates(""Id"" int, ""Value"" decimal(9,6), ""StartDate"" date)
                        ) tv on tv.id = apt.tariff_id and to_char(tv.start_date,'DD.MM.YYYY')='{date:dd.MM.yyyy}' and apt.start_date <= tv.start_date
                        where branch_office_id = 101 order by cities.district_id";
            await con.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
                wr.WriteLine($"\"{dr[0]}\";\"{dr[1]}\";\"{dr[2]}\";{dr[3]};\"{dr[4]}\";\"{dr[5]}\";\"{dr[6]}\";\"\";\"{dr[7]}\";{Convert.ToDecimal(dr[8])}");

            wr.Flush();
            ms.Position = 0;

            return File(ms, "text/csv", "list.csv");
        }
    }
}
