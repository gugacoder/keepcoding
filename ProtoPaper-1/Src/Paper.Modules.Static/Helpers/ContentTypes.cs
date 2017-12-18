using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;

namespace Paper.Modules.Static.Helpers
{
  public static class ContentTypes  
  {
    public static string GetBestMatch(this Controller controller, params string[] contentTypeChoices)
    {
      string accept = controller.Request.Headers["Accept"];
      if (accept == null || accept == "*/*")
        return contentTypeChoices.FirstOrDefault();

      foreach (var choice in contentTypeChoices)
      {
        if (accept.Contains(choice))
          return choice;
      }

      foreach (var json in new[] { "*/json", "text/json", "application/json" })
      {
        if (accept.Contains(json))
        {
          var isJson = contentTypeChoices.Any(x => x.Contains("json"));
          if (isJson)
            return json;
        }
      }

      foreach (var xml in new[] { "*/xml", "text/xml", "application/xml" })
      {
        if (accept.Contains(xml))
        {
          var isXml = contentTypeChoices.Any(x => x.Contains("xml"));
          if (isXml)
            return xml;
        }
      }

      // casos especiais para browsers
      if (accept.Contains("text/html") || accept.Contains("text/plain"))
        return contentTypeChoices.FirstOrDefault();

      return null;
    }
  }
}