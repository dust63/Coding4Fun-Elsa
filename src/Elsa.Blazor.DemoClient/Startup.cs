using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Elsa.Blazor.DemoClient.Data;
using Elsa;
using Elsa.Persistence.EntityFrameworkCore.Extensions;
using Elsa.Persistence.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Elsa.Dashboard.Extensions;
using Elsa.Activities.Http.Extensions;
using Elsa.Activities.Timers.Extensions;
using Elsa.Activities.Console.Extensions;
using Elsa.Activities.Workflows.Extensions;
using Elsa.Activities.UserTask.Extensions;
using Elsa.Activities.Email.Extensions;
using Elsa.Activities.ControlFlow.Extensions;
using NodaTime;
using Elsa.Shared;

namespace Elsa.Blazor.DemoClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services
                     .AddElsa(elsa => elsa
                     .AddEntityFrameworkStores<SqliteContext>(options => options
                                     .UseSqlite(Configuration.GetConnectionString("SqlLite"))))
                    .AddElsaDashboard();

            //Configure dependencies injection for activities
            services
                    .AddTimerActivities(options => options.Configure(x => x.SweepInterval = Duration.FromSeconds(1)))
                    .AddConsoleActivities()
                    .AddWorkflowActivities()
                    .AddEmailActivities()
                    .AddControlFlowActivities()
                    .AddUserTaskActivities()
                    .AddHttpActivities();

            services.AddActivity<CalculatorActivity>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ElsaContext elsaDbcontext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                elsaDbcontext.Database.EnsureCreated();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseHttpActivities();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllers();
            });
        }
    }
}
