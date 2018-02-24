using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Paper.Modules.Static.Controllers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Drive;
using Paper.Modules.Static;

namespace Paper.Host
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddApplicationInsightsTelemetry(Configuration);
      services.AddSingleton<IDrive, DriveHub>();
      services.AddMvc()
        .AddApplicationPart(typeof(Paper.Modules.Static.Controllers.ResourceController).GetTypeInfo().Assembly)
        .AddApplicationPart(typeof(Drive.Controllers.DriveController).GetTypeInfo().Assembly);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment())
      {
        app.UseBrowserLink();
      }

      app.UseStatusCodePagesWithReExecute("/error/{0}");
      app.UseExceptionHandler("/error/500");

      app.UseDefaultFiles();
      app.UseStaticFiles();
      app.UseDirectoryBrowser();

      app.UseMvc();
    }
  }
}
