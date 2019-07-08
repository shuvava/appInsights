using GetMetrics.WebApp.Extensions;
using GetMetrics.WebApp.HealthCheck;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Prometheus;

using Shuvava.Extensions.Metrics.Prometheus;


namespace GetMetrics.WebApp
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
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("X-App-Version");
            });

            services.AddSwaggerDocumentation();

            // add metrics services
            services.Configure<AppMetricsSettings>(Configuration.GetSection("AppMetricsSettings"));
            services.AddHostedService<AppMetrics>();

            services.ConfigureHealthProbe();

            services
                .AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddApiExplorer()
                .AddAuthorization()
                //.AddCors()
                .AddJsonFormatters();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.ConfigureHealthProbe();

            if (env.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
            }

            app.UseHttpMetrics();
            app.UseMetricServer();

            app.UseMvc();
        }
    }
}
