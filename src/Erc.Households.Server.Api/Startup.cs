using AutoMapper;
using EErc.Households.Server.MapperProfiles;
using Erc.Households.Backend.Responses;
using Erc.Households.Server.DataAccess.PostgreSql;
using Erc.Households.Server.Requests;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;

namespace Erc.Households.Server.Api
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
            services.AddAutoMapper(typeof(AccountingPointProfile).Assembly);
            services.AddDbContext<ErcContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("ErcContext")));

            services.AddControllers();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    Configuration.Bind("JwtSettings", options);
                    options.TokenValidationParameters.NameClaimType = "username";
                });

            services.AddTransient<IClaimsTransformation, Helpers.ClaimTransformation>();

            services.AddSingleton<IElasticClient>(provider => new ElasticClient(new System.Uri(Configuration.GetConnectionString("Elasticsearch"))));

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var rabbitMq = Configuration.GetSection("RabbitMQ");
                    cfg.Host(rabbitMq["ConnectionString"], c =>
                    {
                        c.Username(rabbitMq["Username"]);
                        c.Password(rabbitMq["Password"]);
                    });

                    cfg.ConfigureEndpoints(provider);
                }));

                x.AddRequestClient<SearchAccountingPointRequest>();
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

            app.UseCors(builder=>builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
