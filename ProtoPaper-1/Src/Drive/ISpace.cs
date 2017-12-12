using Microsoft.Extensions.Configuration;
using Toolset.Standard;

namespace Drive
{
  public interface ISpace
  {
    string Name { get; }

    IRet<string[]> GetChildren(string path);

    IRet<string> GetTextFile(string path);

    IRet SaveTextFile(string path, string text);
  }
}