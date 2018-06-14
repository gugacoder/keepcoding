using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Paper.Media.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Toolset.Application;
using static System.Environment;

namespace Paper.WebApp.Server
{
  public static class AspNetCoreExtension
  {
    #region IWebHostBuilder

    public static IWebHostBuilder UsePaperHost(this IWebHostBuilder builder, params string[] args)
    {
      var exePath = typeof(AspNetCoreExtension).Assembly.Location;
      var contentPath = Path.GetDirectoryName(exePath);
      builder.UseContentRoot(contentPath);
      return builder;
    }

    #endregion

    #region IServiceCollection

    public static IServiceCollection AddPaperServices(this IServiceCollection services)
    {
      services.AddSingleton<IPaperRendererRegistry, PaperRendererRegistry>();
      return services;
    }

    #endregion

    #region IApplicationBuilder

    public static IApplicationBuilder UsePaperMiddlewares(this IApplicationBuilder app)
    {
      app.Map("/Api/1", PaperPipeline);

      return app;
    }

    private static void PaperPipeline(IApplicationBuilder app)
    {
      app.UseMiddleware<PaperMiddleware>();
    }

    #endregion
  }
}