using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Toolset;

namespace Drive
{
  public class DriveHub
  {
    private List<ISpace> spaces;

    private DriveHub(IEnumerable<ISpace> spaces, IConfiguration configuration)
    {
      this.Configuration = configuration;
      this.spaces = spaces.ToList();
    }

    public IConfiguration Configuration { get; }

    public ISpace GetSpaceIfExists(string spaceName)
    {
      return spaces.FirstOrDefault(x => x.Name == spaceName);
    }

    public ISpace GetSpace(string spaceName)
    {
      var space = GetSpaceIfExists(spaceName);
      if (space == null)
      {
        space = new PhysicalSpace(spaceName);
        this.spaces.Add(space);
      }
      return space;
    }

    public static DriveHub GetHub(IConfiguration configuration)
    {
      configuration = configuration?.GetSection("Drive") ?? configuration;

      var spaces = new List<PhysicalSpace>();
      var spacesConfiguration = configuration?.GetSection("Spaces");

      if (spacesConfiguration != null)
      {
        foreach (var spaceConfiguration in spacesConfiguration.GetChildren())
        {
          var spaceName = spaceConfiguration.Key.ChangeCase(TextCase.PascalCase);
          var space = new PhysicalSpace(spaceName, spaceConfiguration);
          spaces.Add(space);
        }
      }

      return new DriveHub(spaces, configuration);
    }
  }
}