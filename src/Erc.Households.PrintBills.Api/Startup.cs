using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using Dapper;
using Erc.Households.Domain.Shared;
using Erc.Households.PrintBills.Api.Models;
using Erc.Households.PrintBills.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.Configure<List<RecsBillLocation>>(Configuration.GetSection("RecsBillBaseAddresses"));
            services.AddTransient<IDbConnection>(provider => new NpgsqlConnection(Configuration.GetConnectionString("ErcContext")));
           
            services.AddHttpClient<BillService>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    UseDefaultCredentials = true,
                    UseProxy = false
                });

            SqlMapper.AddTypeHandler(typeof(Usage), new JsonObjectTypeHandler());
            SqlMapper.AddTypeHandler(typeof(IList<Calculation>), new JsonObjectTypeHandler());
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
                    var fileType = Enum.Parse<FileType>(context.Request.Query["file_type"]);
                    int.TryParse(context.Request.Query["branch_office_id"], out int branchOfficeId);
                    var commodity = Enum.Parse<Commodity>(context.Request.Query["commodity"]);
                    int.TryParse(context.Request.Query["period_id"], out int periodId);

                    var svc = app.ApplicationServices.GetRequiredService<BillService>();
                    var bill = await svc.GetBillsByPeriod(fileType, branchOfficeId, commodity, periodId);
                    context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    context.Response.Headers.Add("Content-Disposition", $"attachment;FileName={periodId}_{(DateTime.Now-new DateTime(2020,10,1)).Ticks}.xlsx");
                    await bill.CopyToAsync(context.Response.Body);
                });

                endpoints.MapGet("/api/bills/{commodity}/{id:int}", async context =>
                {
                    var type = Enum.Parse<Commodity>(context.Request.RouteValues["commodity"].ToString());
                    var billId = int.Parse(context.Request.RouteValues["id"].ToString());
                    var svc = app.ApplicationServices.GetRequiredService<BillService>();
                    var bill = await svc.GetBillById(type, billId);
                    context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    context.Response.Headers.Add("Content-Disposition", $"attachment;FileName={billId}.xlsx");
                    await bill.CopyToAsync(context.Response.Body);
                });
            });
        }
    }
}
