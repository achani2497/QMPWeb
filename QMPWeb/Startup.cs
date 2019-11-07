using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueMePongo;

namespace QMPWeb
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
            services.AddControllersWithViews();
            services.AddDbContext<DbContext>(opt => opt.UseNpgsql(Configuration.GetConnectionString("heroku")));
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "Eventos",
                    pattern: "{controller=Eventos}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "Guardarropas",
                    pattern: "{controller=Guardarropas}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "EliminarGuardarropas",
                    pattern: "{controller=Guardarropas}/{action=EliminarGuardarropa}/{idGuardarropa}/{idUsuario}");

                endpoints.MapControllerRoute(
                    name: "PrendasPorGuardarropa",
                    pattern: "{controller=Guardarropas}/{action=TraerPrendasDelGuardarropas}/{idGuardarropa}");

                endpoints.MapControllerRoute(
                    name: "Prendas",
                    pattern: "{controller=Prendas}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name:"Login",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}