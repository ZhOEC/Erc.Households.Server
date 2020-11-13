using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Reports
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDbConnection>(provider => new NpgsqlConnection(Configuration.GetConnectionString("ErcContext")));
            services.AddTransient<ReportService>();
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
                endpoints.MapGet("/api/reports/{**slug}", async context =>
                {
                    var svc = app.ApplicationServices.GetRequiredService<ReportService>();
                    var slug = context.Request.RouteValues["slug"].ToString();
                    int.TryParse(context.Request.Query["period_id"], out int periodId);
                    int.TryParse(context.Request.Query["branch_office_id"], out int branchOfficeId);
                    var report = await svc.CreateReportAsync(slug, new Dictionary<string, object> { { nameof(periodId), periodId }, { nameof(branchOfficeId), branchOfficeId } });
                    context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    context.Response.Headers.Add("Content-Disposition", $"attachment;FileName={slug}_{periodId}_{branchOfficeId}_{(long)(DateTime.Now - new DateTime(2020, 10, 1)).TotalSeconds}.xlsx");
                    await report.CopyToAsync(context.Response.Body);
                });

            });
        }
    }
}
