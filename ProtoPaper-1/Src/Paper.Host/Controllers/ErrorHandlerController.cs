using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Paper.Host.Controllers
{
  public class ErrorHandlerController
  {
    [HttpGet("/error/{code}")]
    public IActionResult Error(int code)
    {
      return CreateMessage(code);
    }

    public static ObjectResult CreateMessage(int code)
    {
      var description = ((HttpStatusCode)code).ToString();
      return new ObjectResult(new { Status = code, Description = description });
    }
  }
}