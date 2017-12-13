using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Toolset;

namespace Drive
{
  public interface IDrive
  {
    string[] GetSpaceNames();
    ISpace GetSpace(string spaceName);
    ISpace GetOrCreateSpace(string spaceName);
    void CreateSpace(string spaceName);
  }
}