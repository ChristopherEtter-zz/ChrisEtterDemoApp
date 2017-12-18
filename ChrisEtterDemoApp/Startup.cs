using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChrisEtterDemoApp.Services;
using Microsoft.EntityFrameworkCore;
using ChrisEtterDemoApp.Data.EF;
using ChrisEtterDemoApp.Data;
using Newtonsoft.Json;
using AutoMapper;
using ChrisEtterDemoApp.Data.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChrisEtterDemoApp
{
    public class Startup
    {
        private IConfiguration _config;
        private IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _config = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<StoreUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ChrisEtterDemoAppContext>();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = _config["Tokens:ApiIdentityIssuerKey"],
                        ValidAudience = _config["Tokens:ApiIdentityAudienceKey"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:ApiIdentityKey"]))
                    };
                });

            services.AddDbContext<ChrisEtterDemoAppContext>(cfg =>
            {
                cfg.UseSqlServer(_config.GetConnectionString("ChrisEtterDemoAppConnectionString"));
            });

            services.AddAutoMapper();

            services.AddTransient<IMailService, NullMailService>();
            services.AddScoped<IDataRepository, DataRepository>();
            services.AddTransient<ChrisEtterDemoAppSeeder>();

            services.AddMvc(opt =>
            {
                if(_env.IsProduction())
                {
                    opt.Filters.Add(new RequireHttpsAttribute());
                }
            })
            .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
                
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=App}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "App", action = "Index" });
            });

            if(_env.IsDevelopment())
            {
                //Seed Database
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetService<ChrisEtterDemoAppSeeder>();
                    seeder.Seed().Wait();
                }
            }
            

        }
    }
}
