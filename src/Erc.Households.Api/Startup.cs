using AutoMapper;
using Erc.Households.BranchOfficeManagment.Core;
using Erc.Households.BranchOfficeManagment.EF;
using Erc.Households.DataAccess.Core;
using Erc.Households.DataAccess.EF;
using Erc.Households.EF.PostgreSQL;
using MassTransit;
using MassTransit.MultiBus;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Nest;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Erc.Households.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var cultureInfo = new CultureInfo(Configuration["Culture"]);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("X-Total-Count"));
            });

            services.AddAutoMapper(System.Reflection.Assembly.GetExecutingAssembly());

            services.AddSingleton<IBranchOfficeService>(
                new BranchOfficeManagment.BranchOfficeService(
                    new BranchOfficeDbContext(new DbContextOptionsBuilder<BranchOfficeDbContext>().UseNpgsql(Configuration.GetConnectionString("ErcContext")).Options)
                ));

            services.AddDbContext<ErcContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("ErcContext")));

            services.AddControllers(op => op.InputFormatters.Insert(0, GetJsonPatchInputFormatter()));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    Configuration.Bind("JwtSettings", options);
                    options.TokenValidationParameters.NameClaimType = "username";
                });

            services.AddTransient<IClaimsTransformation, Helpers.ClaimTransformation>();

            services.AddTransient<IUnitOfWork>(provider => new UnitOfWork(provider.GetService<ErcContext>(), provider.GetService<IBus>()));

            services.AddMediatR(typeof(Startup), typeof(CommandHandlers.CloseAccountingPointExemptionHandler), typeof(NotificationHandlers.AccountingPointExemptionClosedHandler)); 

            services.AddSingleton<IElasticClient>(new ElasticClient(new System.Uri(Configuration.GetConnectionString("Elasticsearch"))));

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var rabbitMq = Configuration.GetSection("RabbitMQ");
                    cfg.Host(rabbitMq["ConnectionString"], c =>
                    {
                        c.Username(rabbitMq["UserName"]);
                        c.Password(rabbitMq["Password"]);
                    });
                    
                    cfg.ConfigureEndpoints(provider);
                }));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Erc.Households.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime hostApplicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(); //builder=>builder

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Erc.Households.Api V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson(opt=>opt.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Unspecified)
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }
    }
}
