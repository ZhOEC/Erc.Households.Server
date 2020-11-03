using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Erc.Households.PrintBills.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace Erc.Households.PrintBills.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDbConnection>(provider => new NpgsqlConnection(Configuration.GetConnectionString("ErcContext")));
            services.AddTransient<BillService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/api/bills", async context =>
                {
                    int.TryParse(context.Request.Query["period_id"], out int periodId);
                    var svc = app.ApplicationServices.GetRequiredService<BillService>();
                    var bills = await svc.GetNaturalGasBillsByPeriodAsync(periodId);
                    context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    context.Response.Headers.Add("Content-Disposition", $"attachment;FileName={periodId}.xlsx");
                    await bills.CopyToAsync(context.Response.Body);
                });

                endpoints.MapGet("/api/bills/{id:int}", async context =>
                {
                    var id = context.Request.RouteValues["id"] as int?;

                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
