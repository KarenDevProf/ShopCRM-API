using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using AutoMapper;
using Shop.API.Middlewares;
using Shop.BusinessLayer.Interfaces;
using Shop.BusinessLayer.Services;
using Microsoft.OpenApi.Models;
using Shop.Repositories.Repositories;
using Shop.DAL.Models;
using Shop.Repositories.Repositories.Interfaces;

namespace Shop.API
{
    public class Startup
    {
        #region Constructor

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Properties

        private IConfiguration Configuration { get; set; }

        #endregion

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization();
            services.AddControllers().AddNewtonsoftJson();

            #region Configure Swagger  
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop CRM API", Version = "v1", Description = "API Documentation" });
            });
            #endregion

            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<ShopCRMContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));

            DependenceInjection(services);
            CultureConfiguration(services);
        }

        private void DependenceInjection(IServiceCollection services)
        {
            services.AddScoped<ShopCRMContext>();
            services.AddScoped<IShopServices, ShopServices>();
            services.AddScoped<DbContext, ShopCRMContext>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IGoods, GoodsBl>();
            services.AddScoped<IOrders, OrdersBl>();
        }

        private void CultureConfiguration(IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("en")
                };
                opts.DefaultRequestCulture = new RequestCulture("en-US");
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>()?.Value);

            app.UseRouting();
     
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                          pattern: "{controller=Default}/{action=Index}/{id?}");
            });

            //Seed database
            ShopDbInitializer.Seed(app);
        }
    }
}
