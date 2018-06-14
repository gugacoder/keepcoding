using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Toolset.Application
{
  static class AppConfig
  {
    public static IConfigurationRoot GetDefaultConfiguration()
    {
      var builder =
        new ConfigurationBuilder()
          .SetBasePath(App.Path)
          //.AddXmlFile(path: $"appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile(path: $"appsettings.json", optional: true, reloadOnChange: true)
          //.AddIniFile(path: $"{name}.ini", optional: true, reloadOnChange: true)
          ;

      var configuration = builder.Build();
      return configuration;
    }
  }
}
