using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Toolset;

namespace Media.Service.Proxies
{
  public class Proxy
  {
    private static readonly TimeSpan MaxDelay = TimeSpan.FromMinutes(1);

    public PathString Path { get; set; }

    public Uri ReverseUri { get; set; }

    public DateTime LastSeen { get; set; } = DateTime.Now;

    public bool Enabled { get; set; } = true;

    public bool Available => Enabled && ((DateTime.Now - LastSeen) <= MaxDelay);

    public static Proxy Create(string path, string reverseUri)
    {
      var proxy = new Proxy();

      try
      {
        proxy.Path = path;
      }
      catch (Exception ex)
      {
        throw new Exception(
          $"O proxy pré-configurado não define a propriedade Path corretamente: {path ?? "(null)"}"
          , ex);
      }

      try
      {
        proxy.ReverseUri = new Uri(reverseUri, UriKind.RelativeOrAbsolute);
      }
      catch (Exception ex)
      {
        throw new Exception(
          $"O proxy pré-configurado não define a propriedade ReverseUri corretamente: {reverseUri ?? "(null)"}"
          , ex);
      }

      return proxy;
    }
  }
}