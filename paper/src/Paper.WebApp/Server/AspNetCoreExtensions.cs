using Media.Service.Papers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Paper.Media.Rendering;
using Paper.WebApp.Server.Proxies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Toolset.Application;
using static System.Environment;

namespace Paper.WebApp.Server
{
  public static class AspNetCoreExtensions
  {
    #region IWebHostBuilder

    public static IWebHostBuilder UsePaperWebAppHost(this IWebHostBuilder builder, params string[] args)
    {
      var exePath = typeof(AspNetCoreExtensions).Assembly.Location;
      var contentPath = Path.GetDirectoryName(exePath);
      builder.UseContentRoot(contentPath);
      return builder;
    }

    #endregion

    #region IServiceCollection

    public static IServiceCollection AddPaperWebAppServices(this IServiceCollection services)
    {
      services.AddSingleton<IPaperRendererRegistry, PaperRendererRegistry>();
      services.AddSingleton<IProxyRegistry, ProxyRegistry>();
      return services;
    }

    #endregion

    #region IApplicationBuilder

    public static IApplicationBuilder UsePaperWebAppMiddlewares(this IApplicationBuilder app)
    {
      app.Map("/Api/1", PaperPipeline);

      return app;
    }

    private static void PaperPipeline(IApplicationBuilder app)
    {
      app.UseMiddleware<PaperMiddleware>();
      app.UseMiddleware<ProxyRegistryMiddleware>();
      app.UseMiddleware<ProxyMiddleware>();
    }

    #endregion
  }
}