using System;
using System.IO;
using System.Linq;
using Toolset;

namespace Paper.Modules.Static
{
  public class ResourceStorage
  {
    public class ContentInfo
    {
      public string Content { get; set; }
      public string ContentType { get; set; }
    }

    private readonly string folder;

    public ResourceStorage(string folder)
    {
      this.folder = Path.GetFullPath(folder);
    }

    public string[] GetResourceNames(string parent)
    {
      var dirname = parent.Replace('/', Path.DirectorySeparatorChar);
      var dirpath = Path.Combine(this.folder, dirname);

      if (!Directory.Exists(dirpath))
        return null;

      var folders = (
        from path in Directory.EnumerateDirectories(dirpath)
        select Path.GetFileName(path)
      ).ToArray();

      var files = (
        from name in Directory.EnumerateFiles(dirpath, "*.resource")
        select Path.GetFileNameWithoutExtension(name)
      ).ToArray();

      var names = folders.Concat(files).ToArray();
      return names;
    }

    public ContentInfo Get(string path)
    {
      var name = path.Replace('/', Path.DirectorySeparatorChar);
      var filepath = Path.Combine(this.folder, name + ".resource");
      var infopath = Path.Combine(this.folder, name + ".resource.info");

      if (!File.Exists(filepath))
        return null;

      var content = new ContentInfo();
      content.Content = File.ReadAllText(filepath);

      if (File.Exists(infopath))
      {
        var text = File.ReadLines(infopath).FirstOrDefault();
        content.ContentType = text?.Trim() ?? "";
      }

      return content;
    }

    public void Save(string path, ContentInfo content)
    {
      var name = path.Replace('/', Path.DirectorySeparatorChar);
      var filepath = Path.Combine(this.folder, name + ".resource");
      var infopath = Path.Combine(this.folder, name + ".resource.info");

      var dirpath = Path.GetDirectoryName(filepath);
      if (!Directory.Exists(dirpath))
        Directory.CreateDirectory(dirpath);

      File.WriteAllText(filepath, content.Content);
      File.WriteAllText(infopath, content.ContentType);
    }

    public void Remove(string path)
    {
      var name = path.Replace('/', Path.DirectorySeparatorChar);
      var filepath = Path.Combine(this.folder, name + ".resource");
      var infopath = Path.Combine(this.folder, name + ".resource.info");

      if (!File.Exists(filepath))
        File.Delete(filepath);

      if (!File.Exists(infopath))
        File.Delete(infopath);
    }
  }
}
