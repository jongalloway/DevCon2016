using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace MiddlewareApp
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();

            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Setup options with DI
            services.AddOptions();

            services.Configure<RequestCultureOptions>(options =>
            {
                options.DefaultCulture = new CultureInfo(this.configuration["culture"] ?? "en-GB");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseIISPlatformHandler();

            app.UseRequestCulture();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Hello {CultureInfo.CurrentCulture.DisplayName}");
            });
        }
        
        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
