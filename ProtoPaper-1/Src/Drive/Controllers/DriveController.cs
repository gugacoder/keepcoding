using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Toolset;

namespace Drive.Controllers
{
  [Route("Api/Drive")]
  public class DriveController : Controller
  {
    public DriveController(IDrive drive)
    {
      this.Drive = drive;
    }

    public IDrive Drive { get; }

    [HttpGet("")]
    public IActionResult GetSpaces()
    {
      var spaces = Drive.GetSpaceNames();

      var resources = string.Join(", ",
        spaces.Select(StringExtensions.Quote)
      );

      var json = "{ \"resources\": [ " + resources + " ] }";
      return Content(json, "application/json");
    }

    [HttpGet("{spaceName}/{*resourcePath}")]
    public IActionResult GetResource(string spaceName, string resourcePath)
    {
      var space = Drive.GetOrCreateSpace(spaceName);

      var content = space.GetTextFile(resourcePath);
      if (content.Ok)
        return Content(content.Data, "text/plain");

      var children = space.GetChildren(resourcePath);
      if (children.Ok)
      {
        var resources = string.Join(", ",
          children.Data.Select(StringExtensions.Quote)
        );

        var json = "{ \"resources\": [ " + resources + " ] }";
        return Content(json, "application/json");
      }

      return NotFound();
    }
  }
}