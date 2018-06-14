using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset;

namespace Paper.Media.Rendering.Queries
{
  public class ExposeQueryAttribute : ExposePaperAttribute
  {
    public ExposeQueryAttribute(string path, params string[] alternatePaths)
      : base(QueryRenderer.ContractName, path.AsSingle().Concat(alternatePaths))
    {
    }
  }
}
