using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Paper.Modules.Static.Controllers;

namespace Paper.Modules.Static.Extensions
{
  public static class PaperStaticModuleMvcBuilderExtensions
  {
    public static void AddPaperStaticModule(this IMvcBuilder mvc)
    {
      mvc.AddApplicationPart(typeof(ResourceController).GetTypeInfo().Assembly);
    }
  }
}