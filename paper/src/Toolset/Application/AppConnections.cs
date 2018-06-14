using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolset.Data;

namespace Toolset.Application
{
  class AppConnections : IAppConnections
  {
    private readonly string domain;
    private readonly IConfigurationRoot configuration;

    public AppConnections(string domain)
    {
      this.domain = domain ?? "";
      this.configuration = AppConfig.GetDefaultConfiguration();
    }

    public ConnectionString this[string connectionName]
    {
      get
      {
        if (!App.Domains.Contains(domain))
          return null;

        var keys = KeyNames.GetKeys(domain, "connectionStrings", connectionName);
        foreach (var key in keys)
        {
          var section = configuration.GetSection(key);
          var connectionString = section["connectionString"] ?? section.Value;
          if (connectionString != null)
          {
            var provider = section["provider"] ?? "System.Data.SqlClient";
            var instance = new ConnectionString(connectionName, connectionString, provider);
            return instance;
          }
        }

        return null;
      }
    }
  }
}
