using FlawlessFeedbackFE.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace FlawlessFeedbackFE
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
            var token =
            services.AddControllersWithViews();

            // TODO Add Session services
            //// Required to store Session Data in the Cache
            services.AddDistributedMemoryCache();

            //// Configuring the Session
            services.AddSession(opts =>
            {
                opts.IdleTimeout = TimeSpan.FromMinutes(10);
                opts.Cookie.HttpOnly = true;
                opts.Cookie.IsEssential = true;
            });

            services.AddHttpClient("FlawlessFeedbackHttpClient", c =>
            {
                c.BaseAddress = new Uri(Configuration["WebApiUrl"].ToString());
            });

            services.AddSingleton<LoginCheck>();
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

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}