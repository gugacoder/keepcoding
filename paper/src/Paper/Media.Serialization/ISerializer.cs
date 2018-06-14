using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper.Media.Serialization
{
  public interface ISerializer
  {
    string Serialize(Entity entity);

    void Serialize(Entity entity, Stream output);
  }
}
