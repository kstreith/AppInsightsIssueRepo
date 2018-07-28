using System;
using System.Collections.Generic;
//using CacheManager.Core;
//using Hangfire;
//using Hangfire.Dashboard;
//using Hangfire.SqlServer;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestAppInsightsPerformance.Controllers;

namespace TestAppInsightsPerformance
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder = builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
            services.AddCacheManagerConfiguration(Configuration, cfg =>
            {
                //cfg.WithMicrosoftLogging(services);
            });
            services.AddCacheManager();
            */
            services.Configure<ApplicationInsightsServiceOptions>(Configuration.GetSection("ApplicationInsights"));
            services.AddSingleton(p => new DataClient());
            //services.AddSingleton<CacheDataClient, CacheDataClient>();
            var dbConnection = Configuration["Hangfire:DbConnection"];
            var commandTimeout = Double.Parse(Configuration["Hangfire:CommandTimeout"]);
            services.AddMvc(); //.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddApiVersioning(options =>
            {
                options.Conventions.Controller<QueueController>()
                    .HasApiVersion(new ApiVersion(1, 0));
                options.Conventions.Controller<ValuesController>()
                    .HasApiVersion(new ApiVersion(1, 0));
            });

            services.AddTransient<QueueController, QueueController>();
            /*services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(dbConnection,
                    new SqlServerStorageOptions()
                    {
                        CommandTimeout = TimeSpan.FromSeconds(commandTimeout)
                    });
            });*/
        }

        /*public class AllowAll : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                return true;
            }
        }*/

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider provider)
        {
            loggerFactory.AddApplicationInsights(provider, LogLevel.Information);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            /*else
            {
                app.UseHsts();
            }*/
            /*app.UseHangfireServer(new BackgroundJobServerOptions
            {
                WorkerCount = 2
            });
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new List<IDashboardAuthorizationFilter> { new AllowAll() }
            });*/
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
