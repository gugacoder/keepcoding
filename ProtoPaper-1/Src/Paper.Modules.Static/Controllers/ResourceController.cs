using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;
using static Paper.Modules.Static.ResourceStorage;
using Paper.Modules.Static.Helpers;
using Drive;

namespace Paper.Modules.Static.Controllers
{
  [Route("")]
  [Route("Api/Paper/Modules/Static/Resources")]
  public class ResourceController : Controller
  {
    private readonly ResourceStorage storage;

    public ResourceController(IConfiguration configuration, IDrive drive)
    {
      Configuration = configuration.GetSection("Paper:Modules:Static");
      var folder = Configuration?["Folder"] ?? "./paper/static";
      this.storage = new ResourceStorage(folder);
    }

    public IConfiguration Configuration { get; }
    public IDrive Drive { get; }

    [HttpGet("{*resource}")]
    public IActionResult Get(string resource)
    {
      if (resource == null)
        resource = "";

      var content = this.storage.Get(resource);
      if (content != null)
      {
        var choices =
          (content.ContentType != null)
            ? new[] { content.ContentType }
            : new[] { "text/json", "application/json", "application/vnd.siren+json" };
        
        var validContentType = ContentTypes.GetBestMatch(this, choices);
        if (validContentType == null)
          return StatusCode(406); // Not Acceptable

        return Content(content.Content, validContentType);
      }

      var resourceNames = this.storage.GetResourceNames(resource);
      if (resourceNames != null)
      {
        var names = string.Join(", ", resourceNames.Select(x => "\"" + x + "\""));
        names = "{ \"resources\": [ " + names + " ] }";
      
        var validContentType = ContentTypes.GetBestMatch(this, "text/json", "application/json", "application/vnd.siren+json");
        if (validContentType == null)
          return StatusCode(406); // Not Acceptable

        return Content(names, validContentType);
      }

      return NotFound();
    }

    [Route("{*resource}")]
    [AcceptVerbs("POST", "PUT")]
    public IActionResult Post(string resource)
    {
      if (resource == null)
        return StatusCode(405); // Method Not Allowed

      var content = new ContentInfo();
      content.ContentType = Request.ContentType;

      using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
      {
        content.Content = reader.ReadToEnd();
      }

      this.storage.Save(resource, content);
      return Ok();
    }

    [HttpDelete("{*resource}")]
    public IActionResult Delete(string resource)
    {
      if (resource == null)
        return StatusCode(405); // Method Not Allowed

      this.storage.Remove(resource);
      return Ok();
    }
  }
}
