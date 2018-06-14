using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset;

namespace Paper.Media.Rendering.Entities
{
  public class ExposeFeatureAttribute : ExposePaperAttribute
  {
    public ExposeFeatureAttribute(string path, params string[] alternatePaths)
      : base(FeatureRenderer.ContractName, path.AsSingle().Concat(alternatePaths))
    {
    }
  }
}
