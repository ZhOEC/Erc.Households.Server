using Erc.Households.EF.PostgreSQL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}
namespace Erc.Households.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ErcContext>();
                db.Database.Migrate();
                //db.Database.ExecuteSqlRaw("delete from invoice_payment_item");
                //foreach (var p in db.Payments
                //    .Include(p => p.AccountingPoint.Invoices)
                //        .ThenInclude(i => i.InvoicePaymentItems)
                //        .OrderBy(p=>p.Id)
                //    .ToArray())
                //{
                //    p.ClearInvoicePaymentItems();
                //    p.Process(false);
                //}
                //db.SaveChanges();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
