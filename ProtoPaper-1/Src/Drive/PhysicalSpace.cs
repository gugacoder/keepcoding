using System;
using System.Linq;
using System.Collections.Generic;
using Toolset.Standard;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Drive
{
  public class PhysicalSpace : ISpace
  {
    internal PhysicalSpace(string name)
    {
      var path = "./Drive/" + name;

      this.Name = name;
      this.PhysicalFolder = Path.GetFullPath(path);
    }

    internal PhysicalSpace(string name, string physicalFolder)
    {
      var path = physicalFolder ?? ("./Drive/" + name);

      this.Name = name;
      this.PhysicalFolder = Path.GetFullPath(path);
    }

    internal PhysicalSpace(string name, IConfiguration configuration)
    {
      var path = configuration?["Folder"] ?? ("./Drive/" + name);

      this.Name = name;
      this.PhysicalFolder = Path.GetFullPath(path);
    }

    public string Name { get; }

    public string PhysicalFolder { get; }

    public void CreatePhysicalFolder()
    {
      if (!Directory.Exists(PhysicalFolder))
      {
        Directory.CreateDirectory(PhysicalFolder);
      }
    }

    public IRet<string[]> GetChildren(string path)
    {
      try
      {
        path = GetFullPath(path);

        if (!Directory.Exists(path))
          return Ret.Fail<string[]>("not-found");

        var files =
          from subpath in Directory.EnumerateFiles(path)
          select Path.GetFileName(subpath);

        var dirs =
          from subpath in Directory.EnumerateDirectories(path)
          select Path.GetFileName(subpath) + "/";

        var names = dirs.Concat(files).ToArray();
        return Ret.Succeed(names);
      }
      catch (Exception ex)
      {
        return Ret.Fail<string[]>(ex);
      }
    }

    public IRet<string> GetTextFile(string path)
    {
      try
      {
        path = GetFullPath(path);
        if (!File.Exists(path))
          return Ret.Fail<string>("not-found");

        var text = File.ReadAllText(path);
        return Ret.Succeed(text);
      }
      catch (Exception ex)
      {
        return Ret.Fail<string>(ex);
      }
    }

    public IRet SaveTextFile(string path, string text)
    {
      try
      {
        path = GetFullPath(path);

        var directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
          Directory.CreateDirectory(directory);
        }

        File.WriteAllText(path, text);
        return Ret.Succeed();
      }
      catch (Exception ex)
      {
        return Ret.Fail<string>(ex);
      }
    }

    private string GetFullPath(string path)
    {
      path = path ?? "";
      path = path.Replace('/', Path.DirectorySeparatorChar);
      if (path.StartsWith(Path.DirectorySeparatorChar))
      {
        path = path.Substring(1);
      }
      path = Path.Combine(this.PhysicalFolder, path);
      return Path.GetFullPath(path);
    }
  }
}