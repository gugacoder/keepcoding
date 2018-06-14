using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Paper.Media.Serialization;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Paper.Media;
using Toolset;
using Paper.Media.Rendering;

namespace Paper.WebApp.Server
{
  class PaperMiddleware
  {
    private readonly RequestDelegate next;
    private readonly IPaperRendererRegistry registry;

    public PaperMiddleware(RequestDelegate next, IPaperRendererRegistry registry)
    {
      this.next = next;
      this.registry = registry;
    }

    public async Task Invoke(HttpContext httpContext, IServiceProvider injector)
    {
      var path = httpContext.Request.Path.Value;

      var renderer = registry.FindPaperRenderer(path);
      if (renderer == null)
      {
        await next(httpContext);
        return;
      }

      var req = httpContext.Request;
      var res = httpContext.Response;
      
      var ret = RenderEntity(httpContext, injector, renderer);
      var entity = ret.Data ?? HttpEntity.CreateFromRet(req.GetRequestUri(), ret);

      var contentType = SelectContentType(req);
      var encoding = SelectEncoding(req);

      var serializer = new MediaSerializer(contentType);
      var data = serializer.Serialize(entity);

      res.StatusCode = ret.Status;
      res.ContentType = $"{contentType}; charset={encoding.HeaderName}";

      await res.WriteAsync(data, encoding);
    }

    private Ret<Entity> RenderEntity(HttpContext httpContext, IServiceProvider injector, PaperRendererInfo rendererType)
    {
      try
      {
        var path = httpContext.Request.Path.Value;

        var renderer = (IPaperRenderer)injector.CreateInstance(rendererType.PaperRendererType);
        renderer.PaperType = rendererType.PaperType;
        renderer.PathTemplate = rendererType.PathTemplate;

        var ret = renderer.RenderEntity(httpContext, path);

        return ret;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    private string SelectContentType(HttpRequest req)
    {
      if (req.Query["f"] == "siren" || req.Query["out"] == "siren")
        return "application/vnd.siren+json";

      if (req.Query["f"] == "json" || req.Query["out"] == "json")
        return "application/json";

      if (req.Query["f"] == "xml" || req.Query["out"] == "xml")
        return "application/xml";

      var acceptHeader = (string)req.Headers[HeaderNames.Accept];

      if (acceptHeader.Contains("application/json") || acceptHeader.Contains("*/json"))
        return "application/json";
      if (acceptHeader.Contains("text/json"))
        return "text/json";

      if (acceptHeader.Contains("application/xml") || acceptHeader.Contains("*/xml"))
        return "application/xml";
      if (acceptHeader.Contains("text/xml"))
        return "text/xml";

      return "application/vnd.siren+json";
    }

    private Encoding SelectEncoding(HttpRequest req)
    {
      var charset = (string)SelectOption(req.Headers[HeaderNames.AcceptCharset], "UTF-8");
      try
      {
        return Encoding.GetEncoding(charset);
      }
      catch
      {
        return Encoding.UTF8;
      }
    }

    private string SelectOption(string acceptHeader, string defaultOption)
    {
      if (acceptHeader == null)
        return defaultOption;

      // Aplicando QualityValue para selecionar a melhor opção
      // https://developer.mozilla.org/en-US/docs/Glossary/Quality_values
      var choices =
        from option in acceptHeader.Split(',')
        let tokens = option.Split(':')
        let value = tokens.First().Replace("*", defaultOption)
        let priority = ToDouble(tokens.Skip(1).FirstOrDefault(), 1, 0)
        orderby priority descending
        select option;
      var choice = choices.FirstOrDefault();
      return choice ?? defaultOption;
    }

    private double ToDouble(string text, double defaultValue, double errorValue)
    {
      if (string.IsNullOrWhiteSpace(text))
        return defaultValue;

      double number;
      var ok = double.TryParse(text.Trim(), out number);
      return ok ? number : errorValue;
    }
  }
}