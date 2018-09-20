using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UnityCloudBuildDownloader.Core;

namespace UnityCloudBuildDownloader
{
    public class Startup
    {
        private readonly ILogger _log;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _log = loggerFactory.CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _log.LogInformation("Started with commandline '{0}'", Environment.CommandLine);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMemoryCache();

            services.AddSingleton<IBuildDownloader, HttpBuildDownloader>();
            var settings = Configuration.GetSection("HttpDownloaderSettings");
            services.AddSingleton(new HttpDownloadSettings
            {
                BaseUri = new Uri(settings["BaseUri"], UriKind.Absolute),
                PageSize = int.Parse(settings["PageSize"] ?? "20"),
                AuthKey = settings["AuthKey"],
                GetBuildsUri = new Uri(settings["GetBuildsUri"], UriKind.Relative)
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
