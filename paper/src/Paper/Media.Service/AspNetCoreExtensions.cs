using Media.Service.Papers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Paper.Media.Rendering;
using Paper.Media.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Toolset.Application;
using static System.Environment;

namespace Media.Service
{
  public static class AspNetCoreExtensions
  {
    #region IWebHostBuilder

    public static IWebHostBuilder UsePaperSettings(this IWebHostBuilder builder, params string[] args)
    {
      var baseUri = builder.GetSetting(WebHostDefaults.ServerUrlsKey);
      builder.ConfigureServices(services => RegisterServices(services, baseUri));

      var exePath = typeof(AspNetCoreExtensions).Assembly.Location;
      var contentPath = Path.GetDirectoryName(exePath);
      builder.UseContentRoot(contentPath);

      return builder;
    }

    private static void RegisterServices(IServiceCollection services, string baseUri)
    {
      var settings = new PaperSettings { BaseUri = new Uri(baseUri) };
      services.AddSingleton<IPaperSettings>(settings);
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