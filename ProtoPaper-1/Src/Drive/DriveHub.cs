using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Toolset;

namespace Drive
{
  public class DriveHub : IDrive
  {
    private List<ISpace> spaces;

    public DriveHub(IConfiguration configuration)
    {
      this.Configuration = configuration?.GetSection("Drive") ?? configuration;
      this.spaces = RestoreSpaces(this.Configuration);
    }

    public IConfiguration Configuration { get; }

    public string[] GetSpaceNames()
    {
      return spaces.Select(x => x.Name).ToArray();
    }

    public ISpace GetSpace(string spaceName)
    {
      return spaces.FirstOrDefault(x => x.Name == spaceName);
    }

    public ISpace GetOrCreateSpace(string spaceName)
    {
      CreateSpace(spaceName);
      return GetSpace(spaceName);
    }

    public void CreateSpace(string spaceName)
    {
      var exists = spaces.Any(x => x.Name == spaceName);
      if (exists)
        return;

      var space = new PhysicalSpace(spaceName);
      space.CreatePhysicalFolder();
      this.spaces.Add(space);
    }

    public List<ISpace> RestoreSpaces(IConfiguration configuration)
    {
      var spaces = new List<ISpace>();
      var spacesConfiguration = configuration?.GetSection("Spaces");

      if (spacesConfiguration != null)
      {
        foreach (var spaceConfiguration in spacesConfiguration.GetChildren())
        {
          var spaceName = spaceConfiguration.Key.ChangeCase(TextCase.PascalCase);
          var space = new PhysicalSpace(spaceName, spaceConfiguration);
          space.CreatePhysicalFolder();
          spaces.Add(space);
        }
      }

      return spaces;
    }
  }
}