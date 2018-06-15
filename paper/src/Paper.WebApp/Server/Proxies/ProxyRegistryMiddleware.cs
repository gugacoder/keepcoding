using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Paper.Media;
using Paper.Media.Rendering;
using Paper.Media.Serialization;
using Paper.WebApp.Server.Utilities;
using Toolset;

namespace Paper.WebApp.Server.Proxies
{
  class ProxyRegistryMiddleware
  {
    private readonly RequestDelegate next;
    private readonly IProxyRegistry registry;

    private readonly PathString addPath = new PathString("/Proxies");

    public ProxyRegistryMiddleware(RequestDelegate next, IProxyRegistry registry)
    {
      this.next = next;
      this.registry = registry;
    }

    public async Task Invoke(HttpContext httpContext)
    {
      if (!httpContext.Request.Path.StartsWithSegments(addPath))
      {
        await next(httpContext);
        return;
      }

      try
      {
        var req = httpContext.Request;
        var query = httpContext.Request.Query;

        var path = (string)query["path"];
        var reverseUri = (string)query["reverseUri"];

        if (path == null)
        {
          var ret = HttpEntity.Create(
            route: req.GetRequestUri(),
            status: HttpStatusCode.BadRequest,
            message: "Uso incorreto. Os parâmetros `path' deve ser indicado."
          );
          await SendStatusAsync(httpContext, ret);
          return;
        }

        var isShowInfoOnly = (req.Method == "GET" && reverseUri == null);
        if (isShowInfoOnly)
        {
          await SendProxyAsync(httpContext);
        }
        else
        {
          await EditProxyAsync(httpContext);
        }
      }
      catch (Exception ex)
      {
        await SendStatusAsync(httpContext, Ret.Fail(ex));
      }
    }

    private async Task SendProxyAsync(HttpContext httpContext)
    {
      var req = httpContext.Request;
      var query = httpContext.Request.Query;

      var path = (string)query["path"];

      Proxy proxy = registry.FindExact(path);
      if (proxy != null)
      {
        var status = HttpEntity.Create(
          route: req.GetRequestUri(),
          status: HttpStatusCode.OK
        );

        status.Data.Properties.Add("Path", proxy.Path.ToString());
        status.Data.Properties.Add("ReverseUri", proxy.ReverseUri.ToString());

        await SendStatusAsync(httpContext, status);
      }
      else
      {
        var status = HttpEntity.Create(
          route: req.GetRequestUri(),
          status: HttpStatusCode.NotFound,
          message: $"Proxy não registrado: {path}"
        );
        await SendStatusAsync(httpContext, status);
      }
    }

    private async Task EditProxyAsync(HttpContext httpContext)
    {
      var req = httpContext.Request;
      var query = httpContext.Request.Query;

      var path = (string)query["path"];
      var reverseUri = (string)query["reverseUri"];

      if (reverseUri == null)
      {
        var ret = HttpEntity.Create(
          route: req.GetRequestUri(),
          status: HttpStatusCode.BadRequest,
          message: "Uso incorreto. Os parâmetros `path' e `reverseUri' devem ser indicados."
        );
        await SendStatusAsync(httpContext, ret);
        return;
      }

      bool hasDelete = req.Method.EqualsAny("DELETE", "PUT", "PATCH");
      bool hasEdit = req.Method.EqualsAny("GET", "POST", "PUT", "PATCH");

      Proxy oldProxy = registry.FindExact(path);
      Proxy newProxy;

      try
      {
        newProxy = Proxy.Create(path, reverseUri);
      }
      catch (Exception ex)
      {
        ex.TraceWarning();

        var ret = HttpEntity.Create(
          route: req.GetRequestUri(),
          status: HttpStatusCode.BadRequest,
          message: "Os parâmetros `path' e `reverseUri` não estão corretos."
        );
        await SendStatusAsync(httpContext, ret);
        return;
      }

      if (oldProxy != null && !oldProxy.ReverseUri.Equals(newProxy.ReverseUri))
      {
        var ret = HttpEntity.Create(
          route: req.GetRequestUri(),
          status: HttpStatusCode.BadRequest,
          message: $"Já existe um proxy diferente configurado nesta rota `{oldProxy.Path}'."
        );
        await SendStatusAsync(httpContext, ret);
        return;
      }

      if (hasDelete)
      {
        if (oldProxy != null)
        {
          registry.Remove(path);
        }
      }

      if (hasEdit)
      {
        if (oldProxy != null)
        {
          oldProxy.Enabled = true;
          oldProxy.LastSeen = DateTime.Now;
        }
        else
        {
          registry.Add(newProxy);
        }
      }

      var status = HttpEntity.Create(
        route: req.GetRequestUri(),
        status: HttpStatusCode.OK,
        message: hasEdit ? "Registro de proxy efetuado." : "Registro de proxy removido."
      );

      status.Data.Properties.Add("Path", newProxy.Path.ToString());
      status.Data.Properties.Add("ReverseUri", newProxy.ReverseUri.ToString());

      await SendStatusAsync(httpContext, status);
    }

    private async Task SendStatusAsync(HttpContext httpContext, Ret ret)
    {
      var req = httpContext.Request;
      var res = httpContext.Response;

      var entity =
        (ret.Data as Entity)
        ?? HttpEntity.CreateFromRet(req.GetRequestUri(), ret);

      var contentType = HttpNegotiation.SelectContentType(req);
      var encoding = HttpNegotiation.SelectEncoding(req);

      var serializer = new MediaSerializer(contentType);
      var data = serializer.Serialize(entity);

      res.StatusCode = ret.Status;
      res.ContentType = $"{contentType}; charset={encoding.HeaderName}";

      await res.WriteAsync(data, encoding);
    }
  }
}