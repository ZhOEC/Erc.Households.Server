using Erc.Households.Server.Core;
using Erc.Households.Server.DataAccess;
using Erc.Households.Server.DataAccess.EF;
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

            services.AddTransient<IUnitOfWork>(factory => new UnitOfWork(factory.GetService<ErcContext>(), factory.GetService<IBus>()));

            services.AddSingleton<IElasticClient>(_ => new ElasticClient(new System.Uri(Configuration.GetConnectionString("Elasticsearch"))));

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
